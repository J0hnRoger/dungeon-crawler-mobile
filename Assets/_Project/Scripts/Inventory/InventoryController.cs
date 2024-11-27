using System.Collections.Generic;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryController
    {
        private readonly InventoryModel _model;
        private readonly InventoryView _view;

        private EventBinding<AddItemIntoInventoryEvent> _itemAddedEventBinding;
        
        public InventoryController(InventoryModel model, InventoryView view)
        {
            _model = model;
            _view = view;

            ConnectModel();
            ConnectView();
        }

        public void OnEnable()
        {
            _itemAddedEventBinding = new EventBinding<AddItemIntoInventoryEvent>(OnItemAdded);
            EventBus<AddItemIntoInventoryEvent>.Register(_itemAddedEventBinding);
        }

        private void OnItemAdded(AddItemIntoInventoryEvent itemAddedEvent)
        {
            foreach(var item in itemAddedEvent.Items)
                _model.Items.Add(item);
        }

        private void ConnectModel()
        {
            _model.Items.AnyValueChanged += OnItemChanged;
        }

        private void OnItemChanged(IList<DungeonItem> items)
        {
           // Map to View 
           _view.UpdateItems(items);
        }

        private void ConnectView()
        {
           _view.UpdateItems(_model.Items);
           _view.OnItemDropped += HandleItemEquipped;
        }

        private void HandleItemEquipped(DungeonItem item)
        {
            _model.RemoveItem(item);
        }
    }
}