using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler.Skills;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class Enemy
    {
        public Observable<int> Hp { get; set; }
        public int Damage { get; set; }
        public EnemyData Data { get; private set; }
        public Skill Skill { get; set; }

        public string Name;

        public Enemy(EnemyData data)
        {
            Data = data;
            Hp = new Observable<int>(data.HP);
            Damage = data.Damage;
            Name = data.EnemyName;
            Skill = new Skill(data.Skill);
        }
    }
}