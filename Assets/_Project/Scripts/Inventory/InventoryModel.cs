using System;
using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Common;
using System.Linq;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryModel
    {
        public ObservableList<DungeonItem> Items { get; set; }
        
        public InventoryModel(List<DungeonItem> sampleItems)
        {
            Items = new ObservableList<DungeonItem>(sampleItems);
        }

        public void RemoveItem(DungeonItem dropped)
        {
            var item = Items.FirstOrDefault(i => i == dropped);
            if (item == null)
                throw new Exception($"Item {dropped.Data.Name} not found in the Inventory");
            
            Items.Remove(item);
        }
    }
}