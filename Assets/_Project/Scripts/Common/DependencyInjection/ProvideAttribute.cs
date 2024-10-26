using System;

namespace _Project.Scripts.Common.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : Attribute
    { }
}