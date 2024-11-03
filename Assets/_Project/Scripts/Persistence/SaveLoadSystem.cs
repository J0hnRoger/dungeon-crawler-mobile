using System;
using System.Linq;
using _Project.Scripts.Common;
using _Project.Scripts.Common.EventBus;
using _Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Persistence
{
    public class SaveLoadSystem : Singleton<SaveLoadSystem>
    {
        [SerializeField] public GameData GameData;
        
        private IDataService _dataService;

        protected override void Awake()
        {
            base.Awake();
            _dataService = new FileDataService(new JsonSerializer());
        }

        public void NewGame()
        {
            string date = DateTime.UtcNow.ToString("D");
            GameData = new GameData()
            {
                Name = $"Demo {date}"
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

        public void LoadLastGame()
        {
            var lastGame = _dataService.ListSaves().FirstOrDefault();
            if (lastGame == null)
                throw new Exception("No existing saves, start a new game");
            LoadGame(lastGame);
        }
        
        public void LoadGame(string gameName)
        {
            var data = _dataService.Load(gameName);
            EventBus<GameLoadedEvent>.Raise(new GameLoadedEvent()
            {
                SaveName = gameName,
                GameData = data
            });
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