using System;
using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Common;
using System.Linq;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryModel
    {
        public readonly Dictionary<int, DungeonItem> ItemSlots = new();
        public readonly ObservableList<DungeonItem> Items;
        
        public InventoryModel(ObservableList<DungeonItem> items)
        {
            for(int i = 0; i < items.Count; i++)
                ItemSlots.Add(i, items[i]);
            
            Items = items;
        }

        public void RemoveItem(int slotIndex)
        {
            if (!ItemSlots.ContainsKey(slotIndex))
                throw new Exception($"ItemSlot already empty");
            
            var dropped = ItemSlots[slotIndex];
            ItemSlots.Remove(slotIndex);
            RemoveItem(dropped);
        }
        
        public void AddItem(DungeonItem item, int? slotIndex)
        {
            if (Items.Contains(item))
                throw new Exception($"Item {item.Data.Name} already in the inventory");

            if (slotIndex.HasValue)
            {
                if (!ItemSlots.TryAdd(slotIndex.Value, item))
                    throw new Exception($"ItemSlot {slotIndex} already occupied");
            }
            else
            {
                int lastIndex = (!ItemSlots.Any()) ? 0 : ItemSlots.Last().Key + 1;
                ItemSlots.Add(lastIndex, item);
            }
            Items.Add(item);
        }
        
        private void RemoveItem(DungeonItem dropped)
        {
            var item = Items.FirstOrDefault(i => i == dropped);
            if (item == null)
                throw new Exception($"Item {dropped.Data.Name} not found in the Inventory");
            
            Items.Remove(item);
        }
    }
}