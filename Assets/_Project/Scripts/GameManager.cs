using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common;
using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common.Architecture.Events;
using DungeonCrawler._Project.Scripts.Dungeon;
using DungeonCrawler._Project.Scripts.Dungeon.SO;
using DungeonCrawler._Project.Scripts.Equipment;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Inventory;
using DungeonCrawler._Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.Persistence.Events;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts
{
    /// <summary>
    /// Class responsable de la gestion du cycle de vie d'une session de jeu.
    /// Une session de jeu débute après le MainMenu, et se termine lorsque le joueur quitte le jeu
    /// 1. Chargement des services génériques sans dépendances
    /// 2. Récupération de la partie depuis le SaveLoadSystem 
    /// 3. Chargement du GameStore depuis les données du SaveLoadSystem
    /// 4. Initialisation de tous les Systems dépendants de ces données
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private SaveLoadSystem _saveLoadSystem;

        [SerializeField] private LevelSequenceData _levelSequenceData;

        [SerializeField] private GameStore _gameStore; 
        
        private EventBinding<NewGameEvent> _newGameBinding;
        private EventBinding<ContinueGameEvent> _continueGameBinding;
        private EventBinding<SaveGameEvent> _saveGameBinding;
        private EventBinding<StartNewLevelEvent> _startNewLevelBinding;

        private void OnEnable()
        {
            _newGameBinding = new EventBinding<NewGameEvent>(HandleNewGameRequested);
            EventBus<NewGameEvent>.Register(_newGameBinding);

            _continueGameBinding = new EventBinding<ContinueGameEvent>(HandleLoadGameRequested);
            EventBus<ContinueGameEvent>.Register(_continueGameBinding);

            _saveGameBinding = new EventBinding<SaveGameEvent>(HandleSaveGameEvent);
            EventBus<SaveGameEvent>.Register(_saveGameBinding);
            
            _startNewLevelBinding = new EventBinding<StartNewLevelEvent>(HandleStartNewLevelRequestedAsync);
            EventBus<StartNewLevelEvent>.Register(_startNewLevelBinding);
        }

        private void HandleSaveGameEvent(SaveGameEvent saveGameEvent)
        {
            // Update GameData
            _saveLoadSystem.GameData.Inventory = _gameStore.Items; 
            _saveLoadSystem.GameData.Equipments = _gameStore.Equipments; 
            
            _saveLoadSystem.SaveGame();
        }

        private void OnDisable()
        {
            EventBus<NewGameEvent>.Deregister(_newGameBinding);
            EventBus<ContinueGameEvent>.Deregister(_continueGameBinding);
        }

        private async void HandleNewGameRequested(NewGameEvent newGameEvent)
        {
            var firstLevel = _levelSequenceData.Levels.First();
            var newGameData = new GameData()
            {
                Equipments = new List<EquipmentItem>(),
                Inventory = new List<DungeonItem>(),
                LevelProgressions = new List<LevelProgression>(),
                Name = DateTime.Now.ToString("d-M-yy")
            };
            
            InitStore(newGameData);
            
            await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.EXPLORATION);

            // Charger le premier niveau
            EventBus<LoadLevelEvent>.Raise(new LoadLevelEvent {LevelPrefab = firstLevel});
        }

        private void InitStore(GameData gameData)
        {
            _gameStore.Initialize(gameData);
            EventBus<GameReadyEvent>.Raise(new GameReadyEvent());
        }

        private async void HandleLoadGameRequested(ContinueGameEvent continueGameEvent)
        {
            var gameData = string.IsNullOrWhiteSpace(continueGameEvent.SaveName) 
                ? _saveLoadSystem.LoadLastGame()
                : _saveLoadSystem.LoadGame(continueGameEvent.SaveName);
            
            InitStore(gameData);
            
            await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.HUB);
        }

        private async void HandleStartNewLevelRequestedAsync(StartNewLevelEvent startLevelEvent)
        {
            Debug.Log("HandleStartNewLevelRequestedAsync");
            var currentLevel = _levelSequenceData.Levels
                .First(l => l.name == startLevelEvent.LevelName);

            _gameStore.UpdateProgression(currentLevel.name);

            await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.EXPLORATION);

            // Charger le premier niveau
            EventBus<LoadLevelEvent>.Raise(new LoadLevelEvent { LevelPrefab = currentLevel });
        }

        protected override void AwakeAsSingleton()
        { }
    }
}