using System;
using System.Collections.Generic;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection
{
    public class DependencyRegistration
    {
        public object Instance { get; set; }
        public HashSet<InjectionPoint> InjectionPoints { get; } = new();
    }
}