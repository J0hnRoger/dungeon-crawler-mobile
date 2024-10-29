using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    /// <summary>
    /// Gérer les coffres, ennemis, progression du joueur
    /// </summary>
    public class DungeonSystem : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;
        
        [SerializeField] private int Difficulty;
        [SerializeField] private string Name;

        // [SerializeField]
        // private EnemySO[] Biome;
        
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
            Debug.Log($"{combatEvent} start");
            // Calculation enemy  
            await _sceneLoader.LoadSceneGroup(2);

            // sceneManager.Load(Additif - CombatScene).OnLoadComplete(() => {
            //   EventBus<CombatReadyEvent>(new CombatReadyEvent {
            //        Enemy enemy
            //   }) 
            // OnWin: woss 
            // })
        }
        

        private async void HandleCombatFinished(CombatFinishedEvent combatFinishedEvent)
        {
           Debug.Log("Fin de combat ");
           // Si win --> Exploration
           if (combatFinishedEvent.Win && combatFinishedEvent.IsLastCombat)
           {
               Debug.Log("You Win the level");
               await _sceneLoader.LoadSceneGroup(0);
           }
           // Si loose --> Popup
           else if (combatFinishedEvent.Win)
           {
               Debug.Log("You Win");
               await _sceneLoader.LoadSceneGroup(1);
           }
           else
           {
               await _sceneLoader.LoadSceneGroup(0);
               Debug.Log("You Loose");
           }
        }
    }
}