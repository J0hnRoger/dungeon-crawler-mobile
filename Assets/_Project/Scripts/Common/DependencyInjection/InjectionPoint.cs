using System;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection
{
    public class InjectionPoint
    {
        public MonoBehaviour Target { get; }
        public Action<object> Injector { get; }

        private Action<object> _onResolvedCallback;

        public InjectionPoint(MonoBehaviour target, Action<object> injector)
        {
            Target = target;
            Injector = injector;
        }

        /// <summary>
        /// Constructeur pour les dépendances différées (Deferred<T>)
        /// </summary>
        public InjectionPoint(Deferred<object> deferred, Type dependencyType)
        {
            Target = null; // Pas de cible MonoBehaviour pour les dépendances différées
            Injector = instance => deferred.Resolve(instance); // Résolution via Deferred<T>

            // Abonne un callback lorsque le Deferred<T> est résolu
            deferred.OnLoaded += resolvedInstance =>
            {
                _onResolvedCallback?.Invoke(resolvedInstance);
            };
        }

        /// <summary>
        /// Résout la dépendance en injectant ou en notifiant le callback
        /// </summary>
        public void Resolve(object instance)
        {
            // Injecte dans la cible si applicable
            if (Target != null)
            {
                Injector(instance);
            }

            // Exécute le callback enregistré
            _onResolvedCallback?.Invoke(instance);
        }
    }
}