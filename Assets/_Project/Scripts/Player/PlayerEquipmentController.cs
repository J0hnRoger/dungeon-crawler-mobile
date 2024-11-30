using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Equipment;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Player
{
    public class PlayerEquipmentController
    {
        private readonly Dictionary<BodyPart, SkinnedMeshRenderer> _skinnedMeshRenderers;
        private readonly PlayerEquipmentModel _model;

        public PlayerEquipmentController(Dictionary<BodyPart, SkinnedMeshRenderer> skinnedMeshRenderers,
            PlayerEquipmentModel model)
        {
            _skinnedMeshRenderers = skinnedMeshRenderers;
            _model = model;
            
            _model.OnUnequip += UnequipItem;
            _model.OnEquip += EquipItem;
            
            UpdateEquipmentMeshes();
        }

        private void UpdateEquipmentMeshes()
        {
            foreach (var equipmentByBodyPart in _model.ActiveEquipments)
                EquipItem(equipmentByBodyPart);
        }

        private void EquipItem(EquipmentItem item)
        {
            if (item?.Data == null) return;

            // Unequip current item in slot if exists
            if (_model.IsEquipped(item))
            {
                Debug.Log($"BodyPart {item.Data.BodyPart} already equipped");
                return;
            }

            var bodyPart = _skinnedMeshRenderers[item.Data.BodyPart];
            if (bodyPart == null)
                throw new Exception($"No rendered for the bodyPart: {item.Data.BodyPart}");

            bodyPart.sharedMesh = item.Data.Mesh;
            bodyPart.material = item.Data.Material;
        }

        private void UnequipItem(EquipmentItem item)
        {
            var bodyPart = _skinnedMeshRenderers[item.Data.BodyPart];
            if (bodyPart != null)
            {
                bodyPart.sharedMesh = null;
                bodyPart.material = null;
            }
        }
    }
}