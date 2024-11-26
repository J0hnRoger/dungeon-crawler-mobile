using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Inventory;
using DungeonCrawler._Project.Scripts.Persistence;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts
{
    /// <summary>
    /// Container de toutes les données du jeu 
    /// </summary>
    public class GameStore : MonoBehaviour, IDependencyProvider
    {
        public string Name;
        
        public List<DungeonItem> Items = new();
        public List<DungeonItem> Equipments = new();
        
        public List<LevelProgression> LevelProgressions = new();
        
        public void UpdateProgression(string levelName)
        {
            var levelProgression = LevelProgressions
                .Where(lp => lp.LevelName == levelName).ToList();
            
            if (levelProgression.Any())
            {
                levelProgression.First().NbRuns++;
            }
            else
            {
                LevelProgressions.Add(new LevelProgression() {LevelName = levelName, IsActive = true, NbRuns = 0});
            }
        }
        
        public void Initialize(GameData gameData)
        {
            Equipments.AddRange(gameData.Equipments);
            Items.AddRange(gameData.Inventory);
            LevelProgressions = new List<LevelProgression>();
        }

        [Provide]
        public InventoryStore ProvideInventory()
        {
            return new InventoryStore() { Items = Items };
        }

        [Provide]
        public EquipmentStore ProvideEquipment()
        {
            return new EquipmentStore() { Equipments = Equipments };
        }
    }

    [Serializable]
    public class InventoryStore
    {
        public List<DungeonItem> Items { get; set; }
    }

    [Serializable]
    public class EquipmentStore
    {
        public List<DungeonItem> Equipments { get; set; }
    }
}