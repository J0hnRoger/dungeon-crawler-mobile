using System;
using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    public class EquipmentModel
    {
        public readonly ObservableList<DungeonItem> EquippedItems;

        public EquipmentModel(List<DungeonItem> equippedItems)
        {
            EquippedItems = new ObservableList<DungeonItem>(equippedItems);
        }

        public void EquipItem(DungeonItem equipment)
        {
            if (EquippedItems.Contains(equipment))
                throw new Exception($"Equipment already contains {equipment.Data.Name}");
            
            EquippedItems.Add(equipment);
        }
    }
}