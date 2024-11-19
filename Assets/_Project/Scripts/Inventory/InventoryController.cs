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
            
        }

        private void ConnectView()
        {
            
        }
    }
}