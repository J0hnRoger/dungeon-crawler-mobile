using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Common.DependencyInjection
{
    /// <summary>
    /// Injector is a singleton that injects dependencies into MonoBehaviours.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton<Injector>
    {
        private EventBinding<SceneLoadedEvent> _sceneLoadEventBinding;

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> _registry = new();

        private void OnEnable()
        {
            _sceneLoadEventBinding = new EventBinding<SceneLoadedEvent>(InjectInScene);
            EventBus<SceneLoadedEvent>.Register(_sceneLoadEventBinding);
        }

        private void OnDisable()
        {
            EventBus<SceneLoadedEvent>.Deregister(_sceneLoadEventBinding);
        }

        protected override void Awake()
        {
            base.Awake();
            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (IDependencyProvider provider in providers)
            {
                RegisterProvider(provider);
            }

            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (MonoBehaviour injectable in injectables)
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
                var resolveInstance = Resolve(fieldType);
                if (resolveInstance == null)
                    throw new Exception($"Field Failed to inject {field.Name} into {type.Name}");
                field.SetValue(injectable, resolveInstance);
                Debug.Log($"Field Injected {fieldType.Name} into {type.Name}");
            }

            var injectableMethods = type.GetMethods(BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (MethodInfo method in injectableMethods)
            {
                var requiredParameters = method.GetParameters().Select(parameter => parameter.ParameterType)
                    .ToArray();

                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();

                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                    throw new Exception($"Method Failed to inject {method.Name} into {type.Name}");

                method.Invoke(injectable, resolvedInstances);
                Debug.Log($"Method Injected {method.Name} into {type.Name}");
            }
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

        private object Resolve(Type type)
        {
            _registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
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
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    _registry.Add(returnType, providedInstance);
                    Debug.Log($"Registered {provider.GetType().Name} from {returnType.Name}");
                }
                else
                    throw new Exception($"Provider {provider.GetType().Name} return null for {returnType.Name}");
            }
        }

        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}