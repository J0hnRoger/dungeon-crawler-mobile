using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Inventory.SO;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryModel
    {
        public ObservableList<DungeonItem> Items { get; set; }
        
        public InventoryModel(List<DungeonItemSO> sampleItems)
        {
            Items = new ObservableList<DungeonItem>(sampleItems
                .Select(so => new DungeonItem() {Name = so.Name, Quantity = 1, Data = so})
                .ToList());
        }
    }

    /// <summary>
    /// Représente un objet dans la session de jeu courante
    /// - un objet peut être ramassé
    /// </summary>
    public class DungeonItem
    {
        public string Name { get; set; } 
        public int Quantity { get; set; }
        public DungeonItemSO Data { get; set; }
    }
}