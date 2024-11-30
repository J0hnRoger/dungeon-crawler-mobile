using System;
using DungeonCrawler._Project.Scripts.Equipment;
using DungeonCrawler._Project.Scripts.Equipment.SO;
using DungeonCrawler._Project.Scripts.Inventory.SO;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    /// <summary>
    /// Représente un objet dans la session de jeu courante
    /// - les objets sont loot par des cases coffres
    /// - un objets peut être consommés
    /// - un objets peut être fusionné (manastone)
    /// - un objets peut être équipé
    /// </summary>
    [Serializable]
    public class DungeonItem
    {
        public int Quantity;
        public DungeonItemSO Data;

        protected DungeonItem()
        {
        }

        public static DungeonItem CreateItem(int quantity, DungeonItemSO data)
        {
            if (data is DungeonEquipmentSO equipmentSo)
            {
                return new EquipmentItem { Data = equipmentSo, Quantity = quantity };
            }

            return new DungeonItem { Data = data, Quantity = quantity };
        }
    }
}