using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Equipment;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts
{
    public interface IEquipmentStore
    {
        public ObservableList<EquipmentItem> Equipments { get; }
    }
}