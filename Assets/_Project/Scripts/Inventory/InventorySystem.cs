using System;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Inventory;
using UnityEngine;

namespace DungeonCrawler
{
    public class InventorySystem : MonoBehaviour
    {
        [Inject]
        [SerializeField] 
        private InventoryView _inventoryView;
        
        void Start()
        {
            var model = new InventoryModel();
            var controller = new InventoryController(model, _inventoryView);
        }

        public void Load()
        {
            _inventoryView.ToggleInventoryUI();
        }
    }
}