
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Skills;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatController
    {
        private EventBinding<SkillLaunchedEvent> _playerAttackEventBinding;
        
        private readonly CombatView _view;
        private readonly CombatModel _model;
        
        private readonly CountdownTimer _timer = new CountdownTimer(0);
        private SkillCommand _enemyAutoCommand;

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

        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
            _view.UpdateRadial(_timer.Progress);

            if (!_timer.IsRunning)
            {
                EnemyAttack(_model.Enemy.Damage);
                _timer.Reset(_model.Enemy.Skill.data.cooldown);
                _timer.Start();
            }
        }

        private void EnemyAttack(int enemyDamage)
        {
            _model.Player.Hp.Set(_model.Player.Hp - enemyDamage); 
            if (_model.Enemy.Hp <= 0 || _model.Player.Hp <= 0)
                EventBus<CombatFinishedEvent>.Raise(new CombatFinishedEvent()
                {
                    Win = _model.Enemy.Hp < _model.Player.Hp
                });
        }

        private void OnPlayerAttack(SkillLaunchedEvent skillLaunchedEvent)
        {
            _model.Enemy.Hp.Set(_model.Enemy.Hp - _model.Player.Damage); 
            if (_model.Enemy.Hp <= 0 || _model.Player.Hp <= 0)
                EventBus<CombatFinishedEvent>.Raise(new CombatFinishedEvent()
                {
                    Win = _model.Enemy.Hp < _model.Player.Hp
                });
        }

        private void ConnectView()
        {
           _view.SetEnemyName(_model.Enemy.Name); 
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