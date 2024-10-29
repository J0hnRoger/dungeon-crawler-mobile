using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat.SO
{
    
    [CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
    public class EnemyData : ScriptableObject
    {
        public string EnemyName;
        public int HP;
        public int Damage;
    }
}