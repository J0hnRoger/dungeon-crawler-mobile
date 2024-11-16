using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Skills;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatController
    {
        // Event subscribe
        private EventBinding<SkillLaunchedEvent> _playerAttackEventBinding;
        
        // 
        private readonly CombatView _view;
        private readonly CombatModel _model;
        private readonly float _directHit;
        private readonly float _directHitMultiplier;

        // Services
        private CountdownTimer _timer;
       
        // Communication interne au module
        private SkillCommand _enemyAutoCommand;

        private bool CombatIsFinished => _model.Enemy.Hp <= 0 || _model.Player.Hp <= 0;

        public CombatController(CombatModel model, CombatView view, float directHit, float directHitMultiplier)
        {
            _view = view;
            _model = model;
            _directHit = directHit;
            _directHitMultiplier = directHitMultiplier;
            
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
                return;

            if (_timer == null)
            {
                // Setup Timer for the first round 
                _timer = new CountdownTimer(_model.Enemy.Skill.Data.cooldown);
                _timer.Start();
            }
            
            _timer.Tick(deltaTime);
            _view.UpdateCooldown(_timer.Progress);

            if (!_timer.IsRunning)
            {
                EnemyAttack(_model.Enemy.Damage);
                _timer.Reset(_model.Enemy.Skill.Data.cooldown);
                _timer.Start();
            }
        }

        private void EnemyAttack(int enemyDamage)
        {
            _view.EnemyAttack(_model.Enemy.Skill.Data.animationName);
            _model.Player.Hp.Set(_model.Player.Hp - enemyDamage);
            if (CombatIsFinished)
                EventBus<CombatFinishedEvent>.Raise(
                    new CombatFinishedEvent() {Win = _model.Enemy.Hp < _model.Player.Hp});
        }

        private void OnPlayerAttack(SkillLaunchedEvent skillLaunchedEvent)
        {
            if (CombatIsFinished)
            {
                Debug.Log("[Combat System] Combat finished");
                return;
            }
            
            _view.PlayerAttack(skillLaunchedEvent.AnimationName);
            _view.ShowAttackImpact(skillLaunchedEvent.HitInfo.HitPosition);

            float hitCoefficient = skillLaunchedEvent.HitInfo.DamageCoefficient;

            // si l'appuie est déclenché assez près de la fin du CD du skill, on applique un coup net
            bool isDirectHit = skillLaunchedEvent.Timing <= _directHit;

            if (isDirectHit)
            {
            Debug.Log($"[Combat System] Timing: {skillLaunchedEvent.Timing * 100}% de la fin");
                hitCoefficient *= _directHitMultiplier;
                EventBus<DirectHitEvent>.Raise(new DirectHitEvent()); 
            }

            _model.Enemy.Hp.Set(_model.Enemy.Hp - (int)(_model.Player.Damage * hitCoefficient));
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