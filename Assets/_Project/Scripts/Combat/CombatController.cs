
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatController
    {
        private EventBinding<SkillLaunchedEvent> _playerAttackEventBinding;
        
        private readonly CombatView _view;
        private readonly CombatModel _model;
        
        public CombatController(CombatModel model, CombatView view)
        {
            _view = view;
            _model = model;

            ConnectView();
            ConnectModel();
        }

        public void OnEnable()
        {
            _playerAttackEventBinding = new EventBinding<SkillLaunchedEvent>(OnPlayerAttack);
            EventBus<SkillLaunchedEvent>.Register(_playerAttackEventBinding);
        }

        public void OnDisable()
        {
            EventBus<SkillLaunchedEvent>.Deregister(_playerAttackEventBinding);
        }

        private void OnPlayerAttack(SkillLaunchedEvent skillLaunchedEvent)
        {
            _model.Enemy.Hp.Set(_model.Enemy.Hp - _model.Player.Damage); 
        }

        private void ConnectView()
        {
            
        }
        
        private void ConnectModel()
        {
            _model.Enemy.Hp.ValueChanged += UpdateEnemyHealth;
            _model.Player.Hp.ValueChanged += UpdatePlayerHealth;
        }

        private void UpdatePlayerHealth(int newHealth)
        {
            _view.UpdatePlayerHealth(newHealth);
        }

        private void UpdateEnemyHealth(int newHealth)
        {
            _view.UpdateEnemyHealth(newHealth);
        }
        
    }
}