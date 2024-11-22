using System.Collections.Generic;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryController
    {
        private readonly InventoryModel _model;
        private readonly InventoryView _view;

        public InventoryController(InventoryModel model, InventoryView view)
        {
            _model = model;
            _view = view;

            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
            _model.Items.AnyValueChanged += OnItemChanged;
        }

        private void OnItemChanged(IList<DungeonItem> items)
        {
           // Map to View 
           _view.UpdateItems(items);
        }

        private void ConnectView()
        {
           _view.UpdateItems(_model.Items); 
        }
    }
}