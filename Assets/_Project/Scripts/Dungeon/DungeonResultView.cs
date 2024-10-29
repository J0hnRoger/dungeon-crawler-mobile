using System;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using TMPro;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public class DungeonResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultTitleTxt;

        private EventBinding<CombatResultCalculatedEvent> _onCombatFinishedEventBinding;

        public void OnEnable()
        {
            _onCombatFinishedEventBinding = new EventBinding<CombatResultCalculatedEvent>(SetCombatResult);
            EventBus<CombatResultCalculatedEvent>.Register(_onCombatFinishedEventBinding); 
        }
        
        public void OnDisable()
        {
            EventBus<CombatResultCalculatedEvent>.Deregister(_onCombatFinishedEventBinding); 
        }

        private void SetCombatResult(CombatResultCalculatedEvent combatResultCalculatedEvent)
        {
            _resultTitleTxt.text = (combatResultCalculatedEvent.Win) ? "You WIN!" : "You Loose";
        }
    }
}