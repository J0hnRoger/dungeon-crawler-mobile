using UnityEngine;

namespace _Project.Scripts.Common.ServiceLocator
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        private ServiceLocator _container;
        internal ServiceLocator Container => _container ? _container : (_container = GetComponent<ServiceLocator>());

        private bool _hasBeenBootstrapped;

        public void BootstrapOnDemand()
        {
            if (_hasBeenBootstrapped) return;
            _hasBeenBootstrapped = true;
            Bootstrap();
            
        }

        protected abstract void Bootstrap();
    }

    public class ServiceLocatorGlobalBootstrapper : Bootstrapper
    {
        [SerializeField] private bool dontDestroyOnLoad = true;
        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }

    [AddComponentMenu("ServiceLocator/ServiceLocator Scene")]
    public class ServiceLocatorSceneBootstrapper : Bootstrapper
    {
        protected override void Bootstrap()
        {
            Container.ConfigureForScene();
        }
    }
}