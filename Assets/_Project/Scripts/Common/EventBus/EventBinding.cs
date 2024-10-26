using System;

namespace _Project.Scripts.Common.EventBus
{
    
    public interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
    
    public class EventBinding<T> : IEventBinding<T> where T : IEvent 
    {
        public Action<T> OnEvent { get; set; } = (T _) => { };
        public Action OnEventNoArgs { get; set; } = () => { };

        public EventBinding(Action<T> onEvent)
        {
            this.OnEvent = onEvent;
        }
        
        public EventBinding(Action onEventNoArgs)
        {
            OnEventNoArgs = onEventNoArgs;
        }

        public void Add(Action onEvent) => this.OnEventNoArgs += onEvent;
        public void Remove(Action onEvent) => this.OnEventNoArgs -= onEvent;
        
        public void Add(Action<T> onEvent) => this.OnEvent += onEvent;
        public void Remove(Action<T> onEvent) => this.OnEvent -= onEvent;
    }
}