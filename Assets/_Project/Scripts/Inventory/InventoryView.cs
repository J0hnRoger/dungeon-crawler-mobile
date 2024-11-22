using System.Collections.Generic;
using _Project.Scripts.Common.DependencyInjection;
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
        
        private List<ItemSlot> Items = new();
        
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
            Items.Add(slotComponent);
        }

        public void ToggleInventoryUI()
        {
           _container.SetActive(!_container.activeSelf); 
        }

        public void UpdateItems(IList<DungeonItem> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                Items[i].SetItem(items[i]);
            }
        }
    }
}
