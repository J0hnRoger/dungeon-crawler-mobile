using System;
using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Common;
using System.Linq;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryModel
    {
        public readonly ObservableList<DungeonItem> Items;
        
        public InventoryModel(ObservableList<DungeonItem> items)
        {
            Items = items;
        }

        public void RemoveItem(DungeonItem dropped)
        {
            var item = Items.FirstOrDefault(i => i == dropped);
            if (item == null)
                throw new Exception($"Item {dropped.Data.Name} not found in the Inventory");
            
            Items.Remove(item);
        }

        public void AddItem(DungeonItem item)
        {
            if (Items.Contains(item))
                throw new Exception($"Item {item.Data.Name} already in the inventory");
            Items.Add(item);
        }
    }
}