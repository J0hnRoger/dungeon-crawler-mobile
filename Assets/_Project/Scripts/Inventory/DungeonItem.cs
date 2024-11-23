using System;
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
    }
}