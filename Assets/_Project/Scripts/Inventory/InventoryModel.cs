using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryModel
    {
        public ObservableList<DungeonItem> Items { get; set; }
        
        public InventoryModel(List<DungeonItem> sampleItems)
        {
            Items = new ObservableList<DungeonItem>(sampleItems);
        }
    }
}