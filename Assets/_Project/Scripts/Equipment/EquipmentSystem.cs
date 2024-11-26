using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Persistence;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    /// <summary>
    /// Gère l'assignation d'équipements au joueur via l'UI
    /// </summary>
    public class EquipmentSystem : MonoBehaviour
    {
        [Inject]
        [SerializeField]
        private Deferred<EquipmentView> _deferredEquipmentView = new ();

        private EquipmentController _controller;

        void Start()
        {
            _deferredEquipmentView.OnLoaded += InitView;
        }

        private void InitView(EquipmentView equipmentView)
        {
            equipmentView.ToggleUI();
            var model = new EquipmentModel();
            _controller = new EquipmentController(model, equipmentView);
            _controller.OnEnable();
        }

        public void InitModel(GameData data)
        {
        }
    }
}