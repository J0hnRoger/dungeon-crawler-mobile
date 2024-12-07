using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.DependencyInjection;
using TMPro;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryView : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        private InventoryView provideInventoryView()
        {
            return this;
        }

        [SerializeField] private int _nbSlot = 9;
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private TMP_Text _inventoryTitle;

        private List<ItemSlot> ItemSlots = new();

        public Action<int> OnItemDropped;
        public Action<DungeonItem, int> OnItemPicked;

        void Awake()
        {
            for (int i = 0; i < _nbSlot; i++)
            {
                InstantiateSlot(i);
            }
        }

        private void InstantiateSlot(int index)
        {
            var slot = Instantiate(_slotPrefab, _container.transform);
            var slotComponent = slot.GetComponent<ItemSlot>();
            slotComponent.Index = index;
            slotComponent.OnDropAccepted += ItemSlotDropped;
            slotComponent.OnPickUpAccepted += ItemSlotPicked;
            ItemSlots.Add(slotComponent);
        }

        private void ItemSlotPicked(ItemSlot itemSlot, DungeonItem droppedItem)
        {
            OnItemPicked?.Invoke(droppedItem, itemSlot.Index);
        }

        private void ItemSlotDropped(ItemSlot itemSlot)
        {
            OnItemDropped?.Invoke(itemSlot.Index);
        }

        public void UpdateItems(Dictionary<int, DungeonItem> modelItemSlots)
        {
            foreach (ItemSlot itemSlot in ItemSlots)
                itemSlot.EmptySlot();
            
            foreach (var slot in modelItemSlots)
            {
                ItemSlots[slot.Key].SetItem(slot.Value);
            }
        }

        public void UpdateSlot(int index, DungeonItem item)
        {
            ItemSlots[index].SetItem(item);
        }

        public void UpdateItems(IList<DungeonItem> items)
        {
            for (var i = 0; i < items.Count; i++)
                ItemSlots[i].SetItem(items[i]);
        }

        public void CloseInventory()
        {
            SetInventoryUIEnable(false);
        }

        public void ToggleInventoryUI()
        {
            SetInventoryUIEnable(!_container.gameObject.activeSelf);
        }

        private void SetInventoryUIEnable(bool show)
        {
            _inventoryTitle.gameObject.SetActive(show);
            _container.SetActive(!_container.activeSelf);
        }
    }
}