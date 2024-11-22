using System;
using System.Collections.Generic;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Inventory;
using DungeonCrawler._Project.Scripts.Inventory.SO;
using DungeonCrawler._Project.Scripts.Persistence;
using UnityEngine;

namespace DungeonCrawler
{
    public class InventorySystem : MonoBehaviour
    {
        [Inject]
        private Deferred<InventoryView> _deferredInventoryView = new ();
        
        [Inject] 
        [SerializeField] 
        private List<DungeonItemSO> _sampleItems;
        
        private InventoryController _controller;

        void Start()
        {
            _deferredInventoryView.OnLoaded += InitView;
        }

        private void InitView(InventoryView instance)
        {
            var model = new InventoryModel(_sampleItems);
            _controller = new InventoryController(model, instance);
        }

        public void InitModel(GameData data)
        {
        }
    }
}