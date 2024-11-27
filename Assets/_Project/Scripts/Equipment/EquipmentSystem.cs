using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common.Architecture;
using DungeonCrawler._Project.Scripts.Common.Architecture.Events;
using DungeonCrawler._Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Persistence;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    /// <summary>
    /// Gère l'assignation d'équipements au joueur via l'UI
    /// </summary>
    public class EquipmentSystem : GameSystem
    {
        [Inject]
        [SerializeField]
        private Deferred<EquipmentView> _deferredEquipmentView = new ();

        [Inject]
        private EquipmentStore _store;
        
        private EquipmentController _controller;
        private EquipmentModel _model;

        void Start()
        {
            _deferredEquipmentView.OnLoaded += InitView;
        }

        private void InitView(EquipmentView equipmentView)
        {
            equipmentView.ToggleUI();
            _controller = new EquipmentController(_model, equipmentView);
            _controller.OnEnable();
        }

        public void InitModel(GameData data)
        {
        }

        protected override void OnGameReady(GameReadyEvent gameReadyEvent)
        {
            _model = new EquipmentModel(_store.Equipments);
        }
    }
}