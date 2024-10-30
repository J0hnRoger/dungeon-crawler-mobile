using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Combat.SO;

namespace DungeonCrawler._Project.Scripts.Events
{
    internal class CombatReadyEvent : IEvent
    {
        public EnemyData EnemyData { get; set; }
        public bool IsBoss { get; set; }
    }
}