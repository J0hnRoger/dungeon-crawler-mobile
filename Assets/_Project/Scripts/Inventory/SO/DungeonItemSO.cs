using UnityEngine;
using UnityEngine.UIElements;

namespace DungeonCrawler._Project.Scripts.Inventory.SO
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item")]
    public class DungeonItemSO : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public GameObject ItemPrefab;
    }
}