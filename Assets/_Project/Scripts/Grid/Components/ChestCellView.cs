using System;
using System.Collections.Generic;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Inventory;
using DungeonCrawler._Project.Scripts.UI.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid.Components
{
    public class ChestCellView : MonoBehaviour
    {
        [SerializeField] private List<DungeonItem> _items;
        
        public void PickupItem()
        {
            EventBus<AddItemIntoInventoryEvent>.Raise(new AddItemIntoInventoryEvent()
            {
               Items = _items 
            });
            DialogEvent.ShowNotification("Items added to inventory!");
        }
    }
}