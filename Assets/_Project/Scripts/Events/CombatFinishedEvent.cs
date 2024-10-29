using _Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class CombatFinishedEvent : IEvent
    {
        // Dernier combat du niveau 
        public bool IsLastCombat { get; set; }
        public bool Win { get; set; }
        // public EnemySO EnemyData;
    }
}