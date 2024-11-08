using System.Linq;
using _Project.Scripts.Common;
using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using _Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.Dungeon;
using DungeonCrawler._Project.Scripts.Dungeon.SO;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts
{
    /// <summary>
    /// Class responsable de la gestion du cycle de vie d'une session de jeu.
    /// Une session de jeu débute après le MainMenu, et se termine lorsque le joueur quitte le jeu
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Inject] private SceneLoader _sceneLoader;

        [SerializeField] private LevelSequenceData _levelSequenceData;

        private GameData _gameData;
        
        private EventBinding<NewGameEvent> _newGameBinding;
        private EventBinding<GameLoadedEvent> _loadGameBinding;
        private EventBinding<StartNewLevelEvent> _startNewLevelBinding;

        private void OnEnable()
        {
            _newGameBinding = new EventBinding<NewGameEvent>(HandleNewGameRequested);
            EventBus<NewGameEvent>.Register(_newGameBinding);

            _loadGameBinding = new EventBinding<GameLoadedEvent>(HandleLoadGameRequested);
            EventBus<GameLoadedEvent>.Register(_loadGameBinding);

            _startNewLevelBinding = new EventBinding<StartNewLevelEvent>(HandleStartNewLevelRequestedAsync);
            EventBus<StartNewLevelEvent>.Register(_startNewLevelBinding);
        }

        private void OnDisable()
        {
            EventBus<NewGameEvent>.Deregister(_newGameBinding);
            EventBus<GameLoadedEvent>.Deregister(_loadGameBinding);
        }

        private async void HandleNewGameRequested(NewGameEvent newGameEvent)
        {
            var firstLevel = _levelSequenceData.Levels.First();
            
            _gameData = newGameEvent.GameData;
            _gameData.UpdateProgression(firstLevel.name);

            await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.EXPLORATION);

            // Charger le premier niveau
            EventBus<LoadLevelEvent>.Raise(new LoadLevelEvent {LevelPrefab = firstLevel});
        }

        private async void HandleLoadGameRequested(GameLoadedEvent gameLoadedEvent)
        {
            _gameData = gameLoadedEvent.GameData;
            string currentLevelName = _gameData.LevelProgressions.First(l => l.NbRuns == 0).LevelName;

            var currentLevel = _levelSequenceData.Levels
                .FirstOrDefault(level => level.name == currentLevelName);

            if (currentLevel == null)
                Debug.LogError($"Level not found: {currentLevelName}");

            await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.EXPLORATION);
            // Charger le niveau en cours
            EventBus<LoadLevelEvent>.Raise(new LoadLevelEvent {LevelPrefab = currentLevel});
        }

        private async void HandleStartNewLevelRequestedAsync(StartNewLevelEvent startLevelEvent)
        {
            var firstLevel = _levelSequenceData.Levels
                .Where(l => l.name == startLevelEvent.LevelName)
                .First();

            _gameData.UpdateProgression(firstLevel.name);

            await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.EXPLORATION);

            // Charger le premier niveau
            EventBus<LoadLevelEvent>.Raise(new LoadLevelEvent { LevelPrefab = firstLevel });
        }

    }
}