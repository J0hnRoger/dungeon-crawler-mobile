using DungeonCrawler._Project.Scripts.Common.ScriptableObjects;
using DungeonCrawler._Project.Scripts.Inventory;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts
{
    [CreateAssetMenu(fileName = "New Items Set", menuName = "ScriptableObjects/DungeonItemCollection")]
    public class DungeonItemCollection : RuntimeSetSO<DungeonItem>
    {
    }
}