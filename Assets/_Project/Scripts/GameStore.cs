using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Equipment;
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
        public List<EquipmentItem> Equipments = new();
        
        public List<LevelProgression> LevelProgressions = new();
        
        private EquipmentStore _equipmentStore;

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
            // TODO - trouver un moyen plus élégant que de cast un EquipmentItem au runtime en fonction de son SO
            Equipments.AddRange(gameData.Equipments);
            Items.AddRange(gameData.Inventory.Select(s => DungeonItem.CreateItem(s.Quantity, s.Data)));
            LevelProgressions = new List<LevelProgression>();
        }

        [Provide]
        public InventoryStore ProvideInventory()
        {
            return new InventoryStore()
            {
                Items = new ObservableList<DungeonItem>(Items),
            };
        }

        [Provide]
        public IEquipmentStore ProvideEquipment()
        {
            _equipmentStore = new EquipmentStore()
            {
                Equipments = new ObservableList<EquipmentItem>(Equipments)
            };
            
            return _equipmentStore;
        }
    }

    [Serializable]
    public class InventoryStore 
    {
        public ObservableList<DungeonItem> Items { get; set; }
    }
}