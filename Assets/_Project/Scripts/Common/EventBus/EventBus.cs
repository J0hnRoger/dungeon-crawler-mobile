using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Common.EventBus
{
    public static class EventBus<T> where T : IEvent
    {
        private static readonly HashSet<IEventBinding<T>> _bindings = new HashSet<IEventBinding<T>>();

        public static void Register(EventBinding<T> binding) => _bindings.Add(binding);
        public static void Deregister(EventBinding<T> binding) => _bindings.Remove(binding);

        public static void Raise(T @event)
        {
            foreach (IEventBinding<T> eventBinding in _bindings)
            {
                eventBinding.OnEvent.Invoke(@event);
                eventBinding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} bindings");
            _bindings.Clear();
        }
    }
}