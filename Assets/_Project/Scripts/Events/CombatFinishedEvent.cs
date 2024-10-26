using _Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class CombatFinishedEvent : IEvent
    {
        public bool Win { get; set; }
        // public EnemySO EnemyData;
    }
}