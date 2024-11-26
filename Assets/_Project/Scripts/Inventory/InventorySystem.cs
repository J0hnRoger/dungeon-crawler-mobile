using System;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common.Architecture;
using DungeonCrawler._Project.Scripts.Common.Architecture.Events;
using DungeonCrawler._Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Persistence;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventorySystem : GameSystem 
    {
        [Inject]
        private Deferred<InventoryView> _deferredInventoryView = new ();
        
        [Inject]
        private InventoryStore _store;

        private InventoryModel _model;
        private InventoryController _controller;

        void Start()
        {
            _deferredInventoryView.OnLoaded += InitView;
        }

        protected override void OnGameReady(GameReadyEvent gameReadyEvent)
        {
            _model = new InventoryModel(_store.Items);
        }
        
        private void InitView(InventoryView instance)
        {
            if (_model == null)
                throw new Exception("InventoryModel not initialized");
            instance.ToggleInventoryUI();
            _controller = new InventoryController(_model, instance);
            _controller.OnEnable();
        }

        public void InitModel(GameData data)
        {
        }

    }
}