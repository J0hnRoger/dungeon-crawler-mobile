using System;
using UnityEngine;

namespace _Project.Scripts.Common
{

    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;
        public static bool HasInstance => _instance != null;
        public static T Current => _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name + "AutoCreated";
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        private void Awake() => InitializeSingleton();

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
                return;
            if (_instance == null){
                _instance = this as T;
                AwakeAsSingleton();
            }
            else if (_instance != this)
            {
                Debug.Log($"Duplicate singleton of type {typeof(T)} detected. Destroying duplicate on GameObject '{gameObject.name}'");
                Destroy(gameObject);
            }
        }

        protected abstract void AwakeAsSingleton();
    }
}