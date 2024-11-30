using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Equipment;

namespace DungeonCrawler._Project.Scripts.Player
{
    public class PlayerEquipmentModel
    {
        private readonly IEquipmentStore _store;
        public List<EquipmentItem> ActiveEquipments;
        
        public Action<EquipmentItem> OnUnequip;
        public Action<EquipmentItem> OnEquip;
        
        public PlayerEquipmentModel(IEquipmentStore store)
        {
            _store = store;
            ActiveEquipments = _store.Equipments.ToList();
            _store.Equipments.AnyValueChanged += OnEquipmentChanged;
        }

        public bool IsEquipped(EquipmentItem item)
        {
            return _store.Equipments.All(i => i.Data.BodyPart != item.Data.BodyPart);
        }
        
        private void OnEquipmentChanged(IList<EquipmentItem> updatedEquipmentItems)
        {
            // Unequip items
            foreach (EquipmentItem equipped in ActiveEquipments)
            {
                if (!updatedEquipmentItems.Contains(equipped))
                    OnUnequip?.Invoke(equipped);
            }
            
            foreach (var equipmentItem in updatedEquipmentItems)
                OnEquip?.Invoke(equipmentItem);
        }
        
    }
}