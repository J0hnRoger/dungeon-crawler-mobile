using DungeonCrawler._Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatModel
    {
        public Enemy Enemy;
        public Player Player;

        public CombatModel()
        {
            
        }
    }

    public class Enemy
    {
        public Observable<int> Hp { get; set; }
        public int Damage { get; set; }
        public string Name;

        public Enemy(string name, int hp, int damage)
        {
            Hp = new Observable<int>(hp);
            Damage = damage;
            Name = name;
        }
    }
    
    public class Player
    {
        public Observable<int> Hp { get; set; }
        public int Damage { get; set; }
        public string Name;

        public Player(string name, int hp, int damage)
        {
            Hp = new Observable<int>(hp);
            Damage = damage;
            Name = name;
        }
    }
}