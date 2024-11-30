using System;
using DungeonCrawler._Project.Scripts.Equipment.SO;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    [Serializable]
    public class EquipmentItem : DungeonItem
    {
        public new DungeonEquipmentSO Data
        {
            get
            {
                return (DungeonEquipmentSO)base.Data;
            }
            set
            {
                base.Data = value;
            }
        }
    }
}