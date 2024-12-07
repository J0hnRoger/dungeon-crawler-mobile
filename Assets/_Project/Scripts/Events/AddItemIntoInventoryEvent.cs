﻿using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class AddItemIntoInventoryEvent : IEvent
    {
        public DungeonItem Item;
        public int? SlotIndex;
    }
}