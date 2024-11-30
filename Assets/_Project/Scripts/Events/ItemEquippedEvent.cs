using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Equipment;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class ItemEquippedEvent : IEvent
    {
        public EquipmentItem Item;
    }
}