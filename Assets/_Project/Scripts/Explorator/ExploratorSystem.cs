using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Player
{
    public class ExploratorSystem : MonoBehaviour
    {
        private EventBinding<SkillLaunchedEvent> _skillLaunchedBinding;
        private EventBinding<GridClickedEvent> _gridClickedBinding;

        void OnEnable()
        {
            _gridClickedBinding = new EventBinding<GridClickedEvent>(HandlePlayerMove);
            EventBus<GridClickedEvent>.Register(_gridClickedBinding);
        }

        void OnDisable()
        {
            EventBus<GridClickedEvent>.Deregister(_gridClickedBinding);
        }


        private ExploratorController _controller;

        void Awake()
        {
            _controller = new ExploratorController();
        }

        public void HandlePlayerMove(GridClickedEvent cellClickEvent)
        {
            transform.position = cellClickEvent.Position;
        }
    }
}
