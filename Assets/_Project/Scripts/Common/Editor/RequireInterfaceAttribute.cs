using System;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.Editor
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        public readonly Type InterfaceType;

        public RequireInterfaceAttribute(Type interfaceType)
        {
            Debug.Assert(interfaceType.IsInterface, $"{nameof(interfaceType)} needs to be an interface");
            InterfaceType = interfaceType;
        }
    }
}

