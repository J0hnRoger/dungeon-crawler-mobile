using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Player
{
    public class ExploratorSystem : MonoBehaviour
    {
        private EventBinding<SkillLaunchedEvent> _skillLaunchedBinding;
        private EventBinding<EmptyCellClickedEvent> _gridClickedBinding;

        void OnEnable()
        {
            _gridClickedBinding = new EventBinding<EmptyCellClickedEvent>(HandlePlayerMove);
            EventBus<EmptyCellClickedEvent>.Register(_gridClickedBinding);
        }

        void OnDisable()
        {
            EventBus<EmptyCellClickedEvent>.Deregister(_gridClickedBinding);
        }

        public void HandlePlayerMove(EmptyCellClickedEvent cellClickEvent)
        {
            transform.position = new Vector3(cellClickEvent.Position.x, transform.position.y, cellClickEvent.Position.z);
        }
    }
}
