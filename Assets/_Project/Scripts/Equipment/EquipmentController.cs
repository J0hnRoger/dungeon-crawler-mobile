using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    public class EquipmentController
    {
        private readonly EquipmentModel _model;
        private readonly EquipmentView _view;

        public EquipmentController(EquipmentModel model, EquipmentView view)
        {
            _model = model;
            _view = view;
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
            _model.EquippedItems.AnyValueChanged += HandleEquippedItemChanged;
        }

        private void ConnectView()
        {
            _view.UpdateItems(_model.EquippedItems.ToList());
            _view.OnItemEquipped += HandleItemEquipped;
            _view.OnItemUnequipped += HandleItemUnequipped;
        }

        private void HandleEquippedItemChanged(IList<EquipmentItem> obj)
        {
            // TODO
        }

        private void HandleItemEquipped(EquipmentItem equipment)
        {
            _model.EquipItem(equipment);
        }

        private void HandleItemUnequipped(EquipmentItem equipment)
        {
            _model.UnequipItem(equipment);
        }
        
        public void OnEnable()
        {
            // TODO - Bind equip events 
        }
    }
}