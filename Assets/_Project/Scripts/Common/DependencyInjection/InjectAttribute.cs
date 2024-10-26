using System;

namespace _Project.Scripts.Common.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    { }
}