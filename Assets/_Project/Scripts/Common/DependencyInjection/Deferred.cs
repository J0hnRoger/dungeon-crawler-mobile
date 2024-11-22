using System;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection
{
    public class Deferred<T>
    {
        private T _value;
        private bool _isLoaded;

        public event Action<T> OnLoaded;

        public T Value
        {
            get
            {
                if (!_isLoaded)
                    throw new InvalidOperationException($"Deferred<{typeof(T).Name}> is not yet resolved.");
                return _value;
            }
        }

        public bool IsLoaded => _isLoaded;

        public void Resolve(T value)
        {
            if (_isLoaded) return;

            _value = value;
            _isLoaded = true;

            OnLoaded?.Invoke(value);
        }
    }
}