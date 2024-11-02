using System;
using DungeonCrawler._Project.Scripts.Common;
using UnityEngine;

namespace _Project.Scripts.Persistence
{
    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
    {
        [SerializeField] public GameData _gameData;
        
        private IDataService _dataService;

        protected override void Awake()
        {
            base.Awake();
            _dataService = new FileDataService(new JsonSerializer());
        }

        public void NewGame()
        {
            string date = DateTime.UtcNow.ToString("D");
            _gameData = new GameData()
            {
                Name = "New Game",
                CurrentLevelName = $"Demo {date}" 
            };
        }

        public void SaveGame()
        {
           _dataService.Save(_gameData); 
        }

        public void LoadGame(string gameName)
        {
            _dataService.Load(gameName);
        }
        
        public void DeleteGame(string gameName)
        {
            _dataService.Delete(gameName);
        }

        public void ReloadGame()
        {
            LoadGame(_gameData.Name);
        }
    }
}