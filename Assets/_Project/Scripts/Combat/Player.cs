using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class Player
    {
        public Observable<int> Hp { get; set; }
        public int Damage { get; set; }
        public string Name;
        public PlayerData Data;

        public Player(PlayerData data)
        {
            Data = data;
            Hp = new Observable<int>(data.HP);
            Damage = data.Damage;
        }
    }
}