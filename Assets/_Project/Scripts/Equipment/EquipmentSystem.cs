using System.Linq;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Common.Architecture;
using DungeonCrawler._Project.Scripts.Common.Architecture.Events;
using DungeonCrawler._Project.Scripts.Common.DependencyInjection;
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

        [SerializeField]
        private InterfaceReference<IEquipmentStore> EquipmentStore;

        [SerializeField] 
        private EquipmentView _equipmentView;
        
        [Inject]
        private IEquipmentStore _injectedStore;

        private IEquipmentStore Store => _injectedStore ?? EquipmentStore?.Value;
        
        private EquipmentController _controller;
        private EquipmentModel _model;

        void Start()
        {
            InitModel(Store);
            if (_equipmentView != null)
                InitView(_equipmentView);
            else
                _deferredEquipmentView.OnLoaded += InitView;
        }

        private void InitView(EquipmentView equipmentView)
        {
            equipmentView.ToggleUI();
            _controller = new EquipmentController(_model, equipmentView);
            _controller.OnEnable();
        }

        private void InitModel(IEquipmentStore store)
        {
            _model = new EquipmentModel(store);
        }
        
        protected override void OnGameReady(GameReadyEvent gameReadyEvent)
        {
            InitModel(Store);
        }
    }
}