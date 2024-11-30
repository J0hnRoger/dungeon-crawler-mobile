using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Equipment;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class ItemUnequippedEvent : IEvent
    {
        public EquipmentItem Item;
    }
}