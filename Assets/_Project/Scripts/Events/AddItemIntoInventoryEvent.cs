using System.Collections.Generic;
using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class AddItemIntoInventoryEvent : IEvent
    {
        public List<DungeonItem> Items;
    }
}