using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common.Architecture.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.Architecture
{
    /// <summary>
    /// Représente un system de jeu qui sera intégré à notre framework 
    /// </summary>
    public abstract class GameSystem : MonoBehaviour
    {
        public EventBinding<GameReadyEvent> _onGameReadyEvent;

        public void OnEnable()
        {
            _onGameReadyEvent = new EventBinding<GameReadyEvent>(OnGameReady);
            EventBus<GameReadyEvent>.Register(_onGameReadyEvent); 
        }

        public void OnDisable()
        {
            EventBus<GameReadyEvent>.Deregister(_onGameReadyEvent);
        }
        
        /// <summary>
        /// Méthode du cycle de vie du jeu permettant de déclencher
        /// un callback en réponse à l'état Ready du jeu
        /// </summary>
        /// <param name="gameReadyEvent"></param>
        protected abstract void OnGameReady(GameReadyEvent gameReadyEvent);
    }
}