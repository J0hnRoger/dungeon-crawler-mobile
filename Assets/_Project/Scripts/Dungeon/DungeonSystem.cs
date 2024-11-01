using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    /// <summary>
    /// Gérer les coffres, ennemis, progression du joueur
    /// </summary>
    public class DungeonSystem : MonoBehaviour, IDependencyProvider
    {
        [Inject] 
        public SceneLoader _sceneLoader;
        
        [SerializeField] private int Difficulty;
        [SerializeField] private string Name;

        private CurrentCombat _currentCombat;

        [Provide] 
        public ICurrentCombat CurrentCombat()
        {
            _currentCombat = new CurrentCombat();
            return _currentCombat;
        }

        private EventBinding<CombatStartedEvent> _combatStartBinding;
        private EventBinding<CombatFinishedEvent> _combatFinishedEvent;

        void OnEnable()
        {
            _combatStartBinding = new EventBinding<CombatStartedEvent>(HandleCombatStart);
            EventBus<CombatStartedEvent>.Register(_combatStartBinding);
            _combatFinishedEvent = new EventBinding<CombatFinishedEvent>(HandleCombatFinished);
            EventBus<CombatFinishedEvent>.Register(_combatFinishedEvent);
        }

        void OnDisable()
        {
            EventBus<CombatStartedEvent>.Deregister(_combatStartBinding);
            EventBus<CombatFinishedEvent>.Deregister(_combatFinishedEvent);
        }
        
        private async void HandleCombatStart(CombatStartedEvent combatEvent)
        {
            _currentCombat.Set(combatEvent.EnemyData);
            Debug.Log($"{combatEvent} start");
            // Calculation enemy  
            await _sceneLoader.LoadSceneGroup(2);
            
            EventBus<CombatReadyEvent>.Raise(new CombatReadyEvent()
            {
                EnemyData = combatEvent.EnemyData,
                IsBoss = combatEvent.IsBoss 
            }); 
        }

        private void HandleCombatFinished(CombatFinishedEvent combatFinishedEvent)
        {
            // Calculate Rewards
           EventBus<CombatResultCalculatedEvent>.Raise(new CombatResultCalculatedEvent()
           {
               Win = combatFinishedEvent.Win,
               IsLastCombat = combatFinishedEvent.IsLastCombat
           });
        }
    }
}