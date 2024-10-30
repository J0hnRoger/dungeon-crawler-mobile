using DungeonCrawler._Project.Scripts.Combat.SO;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatModel
    {
        public Enemy Enemy;
        public Player Player;

        public CombatModel(EnemyData enemyData, PlayerData playerData)
        {
            Enemy = new Enemy(enemyData);
            Player = new Player(playerData);
        }
    }
}