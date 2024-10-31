using DungeonCrawler._Project.Scripts.Combat.SO;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public interface ICurrentCombat
    {
        EnemyData Enemy { get; }
        bool IsBoss { get; }
        void Set(EnemyData enemy, bool isBoss = false);
        void Clear();
    }
} 