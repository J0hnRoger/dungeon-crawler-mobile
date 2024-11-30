using System;
using System.Collections.Generic;
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
        
        private List<ItemSlot> Items = new();

        public Action<DungeonItem> OnItemDropped;
        public Action<DungeonItem> OnItemPicked;
        
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
            Items.Add(slotComponent);
        }

        private void ItemSlotPicked(ItemSlot itemSlot)
        {
            OnItemPicked?.Invoke(itemSlot.CurrentItem);
        }

        private void ItemSlotDropped(ItemSlot itemSlot)
        {
            OnItemDropped?.Invoke(itemSlot.CurrentItem);
        }

        public void UpdateItems(IList<DungeonItem> items)
        {
            
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
