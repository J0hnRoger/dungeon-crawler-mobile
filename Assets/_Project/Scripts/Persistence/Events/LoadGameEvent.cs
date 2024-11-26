using _Project.Scripts.Common;
using JetBrains.Annotations;

namespace DungeonCrawler._Project.Scripts.Persistence.Events
{
    /// <summary>
    /// Si le SaveName est "null", alors on charge la dernière sauvegarde 
    /// </summary>
    public class LoadGameEvent : IEvent
    {
        [CanBeNull] public string SaveName { get; set; }
    }
}