using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat.SO
{
    [CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObjects/Player")]
    public class PlayerData : ScriptableObject
    {
        public int HP;
        public int Damage;
    }
}