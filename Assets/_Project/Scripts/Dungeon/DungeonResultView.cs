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

        private EventBinding<DungeonResultEvent> _onDungeonResultEvent;

        private void Start()
        {
            _panel.SetActive(false);
            _close.onClick.AddListener(ClosePopup);
        }

        public void OnEnable()
        {
            _onDungeonResultEvent = new EventBinding<DungeonResultEvent>(ShowDungeonResult);
            EventBus<DungeonResultEvent>.Register(_onDungeonResultEvent);
        }

        public void OnDisable()
        {
            EventBus<DungeonResultEvent>.Deregister(_onDungeonResultEvent);
        }

        private void ShowDungeonResult(DungeonResultEvent dungeonResultEvent)
        {
            _panel.SetActive(true);
        }

        private void ClosePopup()
        {
            _panel.SetActive(false);
            EventBus<DungeonFinishedEvent>.Raise(new DungeonFinishedEvent());
        }
    }
}