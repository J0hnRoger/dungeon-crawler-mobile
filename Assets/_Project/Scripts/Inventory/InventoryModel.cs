using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Inventory.SO;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryModel
    {
        public List<DungeonItem> Items { get; set; }
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