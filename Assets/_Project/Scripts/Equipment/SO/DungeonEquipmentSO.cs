using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Inventory.SO;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Equipment.SO
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "ScriptableObjects/Equipment")]
    public class DungeonEquipmentSO : DungeonItemSO
    {
        public BodyPart BodyPart;
        public Mesh Mesh;
        public Material Material;
    }
}