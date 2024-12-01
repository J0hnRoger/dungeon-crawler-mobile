using System;
using System.Collections.Generic;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Inventory;
using DungeonCrawler._Project.Scripts.UI.Events;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Grid.Components
{
    public class ChestCellView : MonoBehaviour
    {
        [SerializeField] private List<DungeonItem> _items;
        [SerializeField] private Image _iconTemplate;
        [SerializeField] private GameObject _iconContainer;

        private List<Image> _itemIcons = new ();
        
        public void Awake()
        {
            foreach (DungeonItem serializedItem in _items)
            {
                // Convert EquipmentItems 
                var dungeonItem = DungeonItem.CreateItem(serializedItem.Quantity, serializedItem.Data);
                var itemIcon = Instantiate(_iconTemplate, _iconContainer.transform);
                var imageComponent = itemIcon.GetComponent<Image>();
                var buttonComponent = itemIcon.GetComponent<Button>();
                buttonComponent.onClick.AddListener(() => PickupItem(itemIcon, dungeonItem));
                imageComponent.sprite = dungeonItem.Data.Icon;
                
                _itemIcons.Add(imageComponent);
            }
            _iconTemplate.gameObject.SetActive(false);
        }

        private void PickupItem(Image go, DungeonItem pickedItem)
        {
            EventBus<AddItemIntoInventoryEvent>.Raise(new AddItemIntoInventoryEvent()
            {
               Items = new List<DungeonItem>() { pickedItem } 
            });
            Destroy(go.gameObject);
            DialogEvent.ShowNotification($"Items {pickedItem.Data.Name} added to inventory!");
        }
    }
}