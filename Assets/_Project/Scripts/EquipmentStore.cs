using System;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Equipment;
using DungeonCrawler._Project.Scripts.Events;

namespace DungeonCrawler._Project.Scripts
{
    [Serializable]
    public class EquipmentStore : IEquipmentStore
    {
        public ObservableList<EquipmentItem> Equipments { get; set; }
    }
}