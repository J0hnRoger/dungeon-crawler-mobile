using System;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LazyInjectAttribute : Attribute
    {
    }
}