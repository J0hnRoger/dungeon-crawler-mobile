using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    /// <summary>
    /// Façade pour accéder au store global.
    /// Permet de mettre en mémoire des données qui n'ont pas forcément vocation à être persistées
    /// Permet d'effectuer des validations propre au système d'Equippement
    /// </summary>
    public class EquipmentModel
    {
        private readonly IEquipmentStore _equipmentStore;

        public ObservableList<EquipmentItem> EquippedItems => _equipmentStore.Equipments; 
        
        public EquipmentModel(IEquipmentStore equipmentStore)
        {
            _equipmentStore = equipmentStore;
        }

        public void EquipItem(EquipmentItem equipment)
        {
            if (_equipmentStore.Equipments.Contains(equipment))
                throw new Exception($"Equipment already contains {equipment.Data.Name}");
            
            _equipmentStore.Equipments.Add(equipment);
        }

        public void UnequipItem(EquipmentItem equipment)
        {
            var current = _equipmentStore.Equipments.FirstOrDefault(e => e == equipment);
            if (current == null)
                throw new Exception($"Equipment {equipment.Data.Name} not in the equipment");
            
            _equipmentStore.Equipments.Remove(equipment);
        }
    }
}