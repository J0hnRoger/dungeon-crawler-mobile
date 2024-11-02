using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public class DungeonResultView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _close;

        private EventBinding<DungeonFinishedEvent> _onDungeonFinishedEvent;

        private void Start()
        {
            _panel.SetActive(false);
            _close.onClick.AddListener(ClosePopup);
        }

        public void OnEnable()
        {
            _onDungeonFinishedEvent = new EventBinding<DungeonFinishedEvent>(ShowDungeonResult);
            EventBus<DungeonFinishedEvent>.Register(_onDungeonFinishedEvent);
        }

        public void OnDisable()
        {
            EventBus<DungeonFinishedEvent>.Deregister(_onDungeonFinishedEvent);
        }

        private void ShowDungeonResult(DungeonFinishedEvent combatResultCalculatedEvent)
        {
            _panel.SetActive(true);
        }

        private void ClosePopup()
        {
            _panel.SetActive(false);
        }
    }
}