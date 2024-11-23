using System;
using System.Collections.Generic;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Persistence;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [Inject]
        [SerializeField]
        private Deferred<InventoryView> _deferredInventoryView = new ();
        
        [Inject]
        [SerializeField] 
        private List<DungeonItem> _sampleItems;
        
        private InventoryController _controller;

        void Start()
        {
            _deferredInventoryView.OnLoaded += InitView;
        }

        private void InitView(InventoryView instance)
        {
            instance.ToggleInventoryUI();
            var model = new InventoryModel(_sampleItems);
            _controller = new InventoryController(model, instance);
            _controller.OnEnable();
        }

        public void InitModel(GameData data)
        {
        }
    }
}