using _Project.Scripts.Common;
using Vector3 = UnityEngine.Vector3;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class CombatFinishedEvent : IEvent
    {
        public bool Win { get; set; }
        // public EnemySO EnemyData;
    }
}