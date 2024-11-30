using System;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Equipment;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts
{
    [Serializable]
    public class EquipmentStore : IEquipmentStore
    {
        public ObservableList<EquipmentItem> Equipments { get; set; }
        
        private EventBinding<ItemEquippedEvent> _itemEquippedEventBinding;
        private EventBinding<ItemUnequippedEvent> _itemUnequippedEventBinding;

        public void Enable()
        {
            _itemEquippedEventBinding = new EventBinding<ItemEquippedEvent>(HandleItemEquipped);
            EventBus<ItemEquippedEvent>.Register(_itemEquippedEventBinding);
            _itemUnequippedEventBinding = new EventBinding<ItemUnequippedEvent>(HandleItemUnequipped);
            EventBus<ItemUnequippedEvent>.Register(_itemUnequippedEventBinding);
        }

        public void Disable()
        {
            EventBus<ItemEquippedEvent>.Deregister(_itemEquippedEventBinding);
            EventBus<ItemUnequippedEvent>.Deregister(_itemUnequippedEventBinding);
        }
        
        private void HandleItemUnequipped(ItemUnequippedEvent obj)
        {
            Equipments.Add(obj.Item);
        }

        private void HandleItemEquipped(ItemEquippedEvent obj)
        {
            Equipments.Remove(obj.Item);
        }
    }
}