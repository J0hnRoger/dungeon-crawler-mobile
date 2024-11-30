using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Equipment;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Player
{
    [Serializable]
    public class BodyPartRenderer
    {
        public BodyPart bodyPart;
        public SkinnedMeshRenderer renderer;
    }

    public class PlayerSystem : MonoBehaviour
    {
        private EventBinding<EmptyCellClickedEvent> _gridClickedBinding;

        [SerializeField]
        private List<BodyPartRenderer> _bodyPartRenderers;
        
        private Dictionary<BodyPart, SkinnedMeshRenderer> _meshRenderers;

        [SerializeField] private InterfaceReference<IEquipmentStore> EquipmentStore;

        [Inject] private IEquipmentStore _injectedStore;

        private PlayerEquipmentController _equipmentController;

        private IEquipmentStore Store => _injectedStore ?? EquipmentStore?.Value;

        private void Awake()
        {
             _meshRenderers = _bodyPartRenderers.ToDictionary(x => x.bodyPart, x => x.renderer);
        }

        void Start()
        {
            if (Store == null)
                throw new Exception("Store not initialized in PlayerSystem");
            var model = new PlayerEquipmentModel(Store);
            _equipmentController = new PlayerEquipmentController(_meshRenderers, model);
        }

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
            transform.position =
                new Vector3(cellClickEvent.Position.x, transform.position.y, cellClickEvent.Position.z);
        }
    }
}