﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _Project.Scripts.Common.DependencyInjection
{
    /// <summary>
    /// Injector is a singleton that injects dependencies into MonoBehaviours.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton<Injector>
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> _registry = new();

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