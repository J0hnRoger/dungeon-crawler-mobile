using System;
using UnityEngine;

namespace _Project.Scripts.Common.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : PropertyAttribute
    { }
}