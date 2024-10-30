using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Skills;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatController
    {
        private EventBinding<SkillLaunchedEvent> _playerAttackEventBinding;
        private readonly CombatView _view;
        private readonly CombatModel _model;

        private CountdownTimer _timer;
        private SkillCommand _enemyAutoCommand;

        private bool CombatIsFinished => _model.Enemy.Hp <= 0 || _model.Player.Hp <= 0;

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
            if (CombatIsFinished)
            {
                Debug.Log("[Combat System] Combat finished");
                return;
            }

            if (_timer == null)
            {
                // Setup Timer for the first round 
                _timer = new CountdownTimer(_model.Enemy.Skill.Data.cooldown);
                _timer.Start();
            }
            
            _timer.Tick(deltaTime);
            _view.UpdateRadial(_timer.Progress);

            if (!_timer.IsRunning)
            {
                EnemyAttack(_model.Enemy.Damage);
                _timer.Reset(_model.Enemy.Skill.Data.cooldown);
                _timer.Start();
            }
        }

        private void EnemyAttack(int enemyDamage)
        {
            _model.Player.Hp.Set(_model.Player.Hp - enemyDamage);
            if (CombatIsFinished)
                EventBus<CombatFinishedEvent>.Raise(
                    new CombatFinishedEvent() {Win = _model.Enemy.Hp < _model.Player.Hp});
        }

        private void OnPlayerAttack(SkillLaunchedEvent skillLaunchedEvent)
        {
            if (CombatIsFinished)
                return;
            
            _model.Enemy.Hp.Set(_model.Enemy.Hp - _model.Player.Damage);
            if (CombatIsFinished)
                EventBus<CombatFinishedEvent>.Raise(
                    new CombatFinishedEvent() {Win = _model.Enemy.Hp < _model.Player.Hp});
        }

        private void ConnectView()
        {
            _view.SetEnemyName(_model.Enemy.Name);
        }

        private void ConnectModel()
        {
            _model.Enemy.Hp.ValueChanged += UpdateEnemyHealth;
            _model.Player.Hp.ValueChanged += UpdatePlayerHealth;

            UpdateEnemyHealth(_model.Enemy.Data.HP);
            UpdatePlayerHealth(_model.Player.Data.HP);
        }

        private void UpdatePlayerHealth(int newHealth)
        {
            float percent = (float)newHealth / _model.Player.Data.HP;
            _view.UpdatePlayerHealth(percent);
        }

        private void UpdateEnemyHealth(int newHealth)
        {
            float percent = (float)newHealth / _model.Enemy.Data.HP;
            _view.UpdateEnemyHealth(percent);
        }
    }
}