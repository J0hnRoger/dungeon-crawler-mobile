using System;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection
{
    public class InjectionPoint
    {
        public MonoBehaviour Target { get; }
        public Action<object> Injector { get; }

        public InjectionPoint(MonoBehaviour target, Action<object> injector)
        {
            Target = target;
            Injector = injector;
        }
    }
}