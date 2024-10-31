using DungeonCrawler._Project.Scripts.Combat.SO;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public class CurrentCombat : ICurrentCombat
    {
        public EnemyData Enemy { get; private set; }
        public bool IsBoss { get; private set; }

        public void Set(EnemyData enemy, bool isBoss = false)
        {
            Enemy = enemy;
            IsBoss = isBoss;
        }

        public void Clear()
        {
            Enemy = null;
            IsBoss = false;
        }
    }
}