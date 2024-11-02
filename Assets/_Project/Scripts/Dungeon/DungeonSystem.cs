using System;
using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    /// <summary>
    /// Gérer la progression du joueur dans le donjon et l'affichage des UI
    /// Maintien l'état du dungeon: le combat courant, les trésors récoltés,...
    /// </summary>
    public class DungeonSystem : MonoBehaviour, IDependencyProvider
    {
        [Inject] public SceneLoader _sceneLoader;

        [Provide]
        public ICurrentCombat CurrentCombat()
        {
            return _currentCombat;
        }

        [SerializeField] private int Difficulty;
        [SerializeField] private string Name;

        private DungeonController _controller;

        private CurrentCombat _currentCombat;

        private EventBinding<CombatStartedEvent> _combatStartBinding;
        private EventBinding<CombatFinishedEvent> _combatFinishedEvent;
        private EventBinding<DungeonFinishedEvent> _dungeonFinishedEvent;
        private EventBinding<FadeInCompleteEvent> _fadeInCompleteEvent;

        private void Awake()
        {
            _currentCombat = new CurrentCombat();
        }

        void OnEnable()
        {
            _combatStartBinding = new EventBinding<CombatStartedEvent>(HandleCombatStart);
            EventBus<CombatStartedEvent>.Register(_combatStartBinding);
            _combatFinishedEvent = new EventBinding<CombatFinishedEvent>(HandleCombatFinished);
            EventBus<CombatFinishedEvent>.Register(_combatFinishedEvent);
            _dungeonFinishedEvent = new EventBinding<DungeonFinishedEvent>(HandleDungeonFinished);
            EventBus<DungeonFinishedEvent>.Register(_dungeonFinishedEvent);
            _fadeInCompleteEvent = new EventBinding<FadeInCompleteEvent>(HandleFadeInComplete);
            EventBus<FadeInCompleteEvent>.Register(_fadeInCompleteEvent);
        }

        void OnDisable()
        {
            EventBus<CombatStartedEvent>.Deregister(_combatStartBinding);
            EventBus<CombatFinishedEvent>.Deregister(_combatFinishedEvent);
            EventBus<FadeInCompleteEvent>.Deregister(_fadeInCompleteEvent);
            EventBus<DungeonFinishedEvent>.Deregister(_dungeonFinishedEvent);
        }
        
        public void Start()
        {
            _controller = new DungeonController.Builder()
                .Build();
        }

        private async void HandleDungeonFinished(DungeonFinishedEvent obj)
        {
            if (_sceneLoader != null)
                await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.HUB);
        }
        
        private async void HandleFadeInComplete(FadeInCompleteEvent obj)
        {
            if (_sceneLoader != null)
                await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.HUB);
        }

        private async void HandleCombatStart(CombatStartedEvent combatEvent)
        {
            _currentCombat.Set(combatEvent.EnemyData);

            if (_sceneLoader != null)
                await _sceneLoader.LoadSceneGroup(2);

            EventBus<CombatReadyEvent>.Raise(new CombatReadyEvent()
            {
                EnemyData = combatEvent.EnemyData, IsBoss = combatEvent.IsBoss
            });
        }

        private async void HandleCombatFinished(CombatFinishedEvent combatFinishedEvent)
        {
            _currentCombat.Clear();
            if (_sceneLoader != null)
                await _sceneLoader.LoadSceneGroup((int)DungeonCrawlerScenes.EXPLORATION);

            // Calculate Rewards
            EventBus<CombatResultCalculatedEvent>.Raise(new CombatResultCalculatedEvent()
            {
                Win = combatFinishedEvent.Win, IsLastCombat = combatFinishedEvent.IsLastCombat
            });
        }
    }
}