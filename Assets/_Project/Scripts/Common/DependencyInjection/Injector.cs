using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

namespace _Project.Scripts.Common.DependencyInjection
{
    /// <summary>
    /// Injector is a singleton that injects dependencies into MonoBehaviours.
    /// or destroy it if exists
    /// - TODO - support lazy loading
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton<Injector>
    {
        private EventBinding<SceneLoadedEvent> _sceneLoadEventBinding;

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        private readonly Dictionary<Type, DependencyRegistration> _registry = new();

        private void OnEnable()
        {
            _sceneLoadEventBinding = new EventBinding<SceneLoadedEvent>(InjectInScene);
            EventBus<SceneLoadedEvent>.Register(_sceneLoadEventBinding);
        }

        private void OnDisable()
        {
            EventBus<SceneLoadedEvent>.Deregister(_sceneLoadEventBinding);
        }

        protected override void AwakeAsSingleton()
        {
            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (IDependencyProvider provider in providers)
            {
                RegisterProvider(provider);
            }

            var injectables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(IsInjectable);

            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        private void InjectInScene(SceneLoadedEvent sceneLoadedEvent)
        {
            if (!sceneLoadedEvent.LoadedScene.isLoaded)
                throw new Exception($"[Injector]: Scene {sceneLoadedEvent.LoadedScene.name} is not fully loadled");

            var rootObjects = sceneLoadedEvent.LoadedScene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                // Search new Providers in scene
                var providers = rootObject.GetComponentsInChildren<IDependencyProvider>(true);
                foreach (IDependencyProvider provider in providers)
                {
                    RegisterProvider(provider);
                }

                var injectables = rootObject.GetComponentsInChildren<MonoBehaviour>(true)
                    .Where(IsInjectable);

                foreach (var injectable in injectables)
                {
                    Inject(injectable);
                }
            }
        }

        private void Inject(MonoBehaviour injectable)
        {
            var type = injectable.GetType();
            var injectableFields = type.GetFields(BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (FieldInfo field in injectableFields)
            {
                var fieldType = field.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Deferred<>))
                {
                    // Gestion des dépendances différées
                    var deferredType = fieldType.GetGenericArguments()[0];

                    if (!_registry.TryGetValue(deferredType, out var registration))
                    {
                        registration = new DependencyRegistration();
                        _registry[deferredType] = registration;
                    }

                    // Crée une instance de Deferred<T> pour le champ
                    var deferredInstance = Activator.CreateInstance(fieldType);
                    field.SetValue(injectable, deferredInstance);

                    // Ajoute un InjectionPoint pour notifier le Deferred<T> lorsque la dépendance est résolue
                    var injectionPoint = new InjectionPoint(injectable, instance =>
                    {
                        var resolveMethod = fieldType.GetMethod("Resolve");
                        resolveMethod?.Invoke(deferredInstance, new[] {instance});
                    });

                    registration.InjectionPoints.Add(injectionPoint);

                    Debug.Log($"[Injector] Deferred<{deferredType.Name}> created for {type.Name}");
                }
                else
                {
                    // Vérifie si la dépendance est déjà disponible dans le registre
                    if (_registry.TryGetValue(fieldType, out var registration) && registration.Instance != null)
                    {
                        // Injecte immédiatement la dépendance
                        field.SetValue(injectable, registration.Instance);
                        Debug.Log($"[Injector] Field {fieldType.Name} injected into {type.Name}");
                    }
                    else
                    {
                        // Si la dépendance n'est pas disponible, crée un WeakPoint
                        Debug.LogWarning(
                            $"[Injector] Missing dependency for {fieldType.Name} in {type.Name}. Creating WeakPoint...");

                        if (!_registry.TryGetValue(fieldType, out registration))
                        {
                            registration = new DependencyRegistration();
                            _registry[fieldType] = registration;
                        }

                        // Ajoute un WeakPoint pour ce champ
                        var weakPoint = new InjectionPoint(injectable, instance =>
                        {
                            field.SetValue(injectable, instance);
                            Debug.Log(
                                $"[Injector] WeakPoint resolved: Field {fieldType.Name} injected into {type.Name}");
                        });
                        registration.InjectionPoints.Add(weakPoint);
                    }
                }
            }

            var injectableMethods = type.GetMethods(BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (MethodInfo method in injectableMethods)
            {
                var parameters = method.GetParameters();

                var resolvedInstances = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramType = parameters[i].ParameterType;

                    if (!_registry.TryGetValue(paramType, out var registration) || registration.Instance == null)
                    {
                        Debug.LogWarning(
                            $"[Injector] Missing dependency for parameter {paramType.Name} in method {method.Name} of {type.Name}");
                        resolvedInstances[i] = null;
                        continue;
                    }

                    resolvedInstances[i] = registration.Instance;
                }

                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    Debug.LogWarning(
                        $"[Injector] Cannot invoke method {method.Name} in {type.Name} due to unresolved dependencies.");
                    continue;
                }

                try
                {
                    method.Invoke(injectable, resolvedInstances);
                    Debug.Log($"[Injector] Method {method.Name} invoked in {type.Name}");
                }
                catch (TargetInvocationException ex)
                {
                    Debug.LogError(
                        $"[Injector] Error invoking method {method.Name} in {type.Name}: {ex.InnerException?.Message}");
                }
            }
        }

        private DependencyRegistration Resolve(Type type)
        {
            _registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }

        public void ValidateDependencies()
        {
            var monoBehaviours = FindMonoBehaviours();
            var providers = monoBehaviours.OfType<IDependencyProvider>();
            var providedDependencies = GetProvidedDependencies(providers);

            var invalidDependencies = monoBehaviours
                .SelectMany(mb => mb.GetType().GetFields(BindingFlags), (mb, field) => new {mb, field})
                .Where(t => Attribute.IsDefined(t.field, typeof(InjectAttribute)))
                .Where(t => !providedDependencies.Contains(t.field.FieldType) && t.field.GetValue(t.mb) == null)
                .Select(t =>
                    $"[Validation] {t.mb.GetType().Name} is missing dependency {t.field.FieldType.Name} on GameObject {t.mb.gameObject.name}");

            var invalidDependencyList = invalidDependencies.ToList();

            if (!invalidDependencyList.Any())
            {
                Debug.Log("[Validation] All dependencies are valid.");
            }
            else
            {
                Debug.LogError($"[Validation] {invalidDependencyList.Count} dependencies are invalid:");
                foreach (var invalidDependency in invalidDependencyList)
                {
                    Debug.LogError(invalidDependency);
                }
            }
        }

        HashSet<Type> GetProvidedDependencies(IEnumerable<IDependencyProvider> providers)
        {
            var providedDependencies = new HashSet<Type>();
            foreach (var provider in providers)
            {
                var methods = provider.GetType().GetMethods(BindingFlags);

                foreach (var method in methods)
                {
                    if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                    var returnType = method.ReturnType;
                    providedDependencies.Add(returnType);
                }
            }

            return providedDependencies;
        }

        public void ClearDependencies()
        {
            foreach (var monoBehaviour in FindMonoBehaviours())
            {
                var type = monoBehaviour.GetType();
                var injectableFields = type.GetFields(BindingFlags)
                    .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

                foreach (var injectableField in injectableFields)
                {
                    injectableField.SetValue(monoBehaviour, null);
                }
            }

            Debug.Log("[Injector] All injectable fields cleared.");
        }


        static bool IsInjectable(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(BindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(BindingFlags);
            foreach (MethodInfo method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;
                var returnType = method.ReturnType;
                if (returnType == typeof(void))
                {
                    Debug.LogWarning(
                        $"[Injector] Method {method.Name} in {provider.GetType().Name} marked as [Provide] cannot return void.");
                    continue;
                }

                var providedInstance = method.Invoke(provider, null);

                if (providedInstance != null)
                {
                    Register(returnType, providedInstance);
                    Debug.Log($"Registered {provider.GetType().Name} from {returnType.Name}");
                }
                else
                    throw new Exception($"Provider {provider.GetType().Name} return null for {returnType.Name}");
            }
        }

        /// <summary>
        /// Enregistre une instance pour un type donné.
        /// </summary>
        private void Register(Type type, object instance)
        {
            if (!_registry.TryGetValue(type, out var registration))
            {
                registration = new DependencyRegistration();
                _registry[type] = registration;
            }

            registration.Instance = instance;
            UpdateDependencies(registration);
        }

        /// <summary>
        /// Mets à jour les dépendances manquantes lorsqu'une nouvelle dépendance est enregistrée.
        /// </summary>
        private static void UpdateDependencies(DependencyRegistration registration)
        {
            foreach (var injectionPoint in registration.InjectionPoints.ToList())
            {
                injectionPoint.Injector(registration.Instance);
            }
        }

        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}