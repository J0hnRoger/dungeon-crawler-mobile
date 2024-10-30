using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler.Skills;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatModel
    {
        public Enemy Enemy;
        public Player Player;

        public CombatModel(EnemyData enemyData, PlayerData player)
        {
            Enemy = new Enemy(enemyData.EnemyName, enemyData.HP, enemyData.Damage, enemyData.Skill);
            Player = new Player(player.HP, player.Damage);
        }
    }

    public class Enemy
    {
        public Observable<int> Hp { get; set; }
        public int Damage { get; set; }
        public Skill Skill { get; set; }

        public string Name;

        public Enemy(string name, int hp, int damage, SkillData skillData)
        {
            Hp = new Observable<int>(hp);
            Damage = damage;
            Name = name;
            Skill = new Skill(skillData);
        }
    }
    
    public class Player
    {
        public Observable<int> Hp { get; set; }
        public int Damage { get; set; }
        public string Name;

        public Player(int hp, int damage)
        {
            Hp = new Observable<int>(hp);
            Damage = damage;
        }
    }
}