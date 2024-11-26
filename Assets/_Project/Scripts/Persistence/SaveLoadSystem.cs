using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common;
using _Project.Scripts.Common.EventBus;
using _Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.Inventory;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Persistence
{
    public class SaveLoadSystem : Singleton<SaveLoadSystem>
    {
        [SerializeField] public GameData GameData;
        
        private IDataService _dataService;

        protected override void AwakeAsSingleton()
        {
            _dataService = new FileDataService(new NewtonsoftJsonSerializer());
        }

        public void NewGame()
        {
            string date = DateTime.UtcNow.ToString("D");
            GameData = new GameData()
            {
                Name = $"Demo {date}",
                Inventory = new List<DungeonItem>(),
                Equipments = new List<DungeonItem>()
            };
            
            EventBus<NewGameEvent>.Raise(new NewGameEvent()
            {
               GameData = GameData
            });
        }

        public void SaveGame()
        {
           _dataService.Save(GameData); 
        }

        public GameData LoadLastGame()
        {
            var lastGame = _dataService.ListSaves().FirstOrDefault();
            if (lastGame == null)
                throw new Exception("No existing saves, start a new game");
            return LoadGame(lastGame);
        }
        
        public GameData LoadGame(string gameName)
        {
            var data = _dataService.Load(gameName);
            return data;
        }
        
        public void DeleteGame(string gameName)
        {
            _dataService.Delete(gameName);
        }

        public void ReloadGame()
        {
            LoadGame(GameData.Name);
        }
    }
}