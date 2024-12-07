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
            _model.AddItem(itemAddedEvent.Item, itemAddedEvent.SlotIndex);
        }

        private void ConnectModel()
        {
            _model.Items.AnyValueChanged += OnItemChanged;
        }

        private void OnItemChanged(IList<DungeonItem> items)
        {
           _view.UpdateItems(_model.ItemSlots); 
        }

        private void ConnectView()
        {
           _view.UpdateItems(_model.Items);
           _view.OnItemDropped += HandleItemDropped;
           _view.OnItemPicked += HandleItemPickup;
        }

        private void HandleItemPickup(DungeonItem item, int index)
        {
            _model.AddItem(item, index);
        }

        private void HandleItemDropped(int index)
        {
            _model.RemoveItem(index);
        }
    }
}