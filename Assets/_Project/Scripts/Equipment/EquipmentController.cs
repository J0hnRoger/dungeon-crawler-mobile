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
        }

        public void OnEnable()
        {
            // TODO - Bind equip events 
        }
    }
}