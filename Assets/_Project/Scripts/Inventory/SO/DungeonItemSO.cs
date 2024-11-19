using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Inventory.SO
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item")]
    public class DungeonItemSO : ScriptableObject
    {
       public string Name { get; set; } 
       public Image Icon { get; set; }
       public GameObject ItemPrefab { get; set; }
    }
}