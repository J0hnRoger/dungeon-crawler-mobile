using DungeonCrawler._Project.Scripts.Inventory;
using UnityEngine;

namespace DungeonCrawler
{
    public class InventorySystem
    {
        [SerializeField] private InventoryView _inventoryView;
        void Start()
        {
            var model = new InventoryModel();
            var controller = new InventoryController(model, _inventoryView);
        }
    }
}