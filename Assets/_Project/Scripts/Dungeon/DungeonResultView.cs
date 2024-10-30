using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public class DungeonResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultTitleTxt;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _close;
        
        private EventBinding<CombatResultCalculatedEvent> _onCombatFinishedEventBinding;

        private void Awake()
        {
            _panel.SetActive(false);
            _close.onClick.AddListener(ClosePopup);
        }

        public void OnEnable()
        {
            _onCombatFinishedEventBinding = new EventBinding<CombatResultCalculatedEvent>(ShowCombatResult);
            EventBus<CombatResultCalculatedEvent>.Register(_onCombatFinishedEventBinding); 
        }
        
        public void OnDisable()
        {
            EventBus<CombatResultCalculatedEvent>.Deregister(_onCombatFinishedEventBinding); 
        }

        private void ShowCombatResult(CombatResultCalculatedEvent combatResultCalculatedEvent)
        {
            _panel.SetActive(true);
            _resultTitleTxt.text = (combatResultCalculatedEvent.Win) ? "You WIN!" : "You Loose";
        }
        

        private void ClosePopup()
        {
            _panel.SetActive(false);
        }
    }
}