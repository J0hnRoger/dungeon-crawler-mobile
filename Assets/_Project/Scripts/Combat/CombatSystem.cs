using System;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Dungeon;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Inject] 
        private ICurrentCombat _currentCombatState;

        [SerializeField] private CombatView _view;
        
        [SerializeField] private EnemyData _enemyData;
        
        [SerializeField] private PlayerData _playerData;
        
        private CombatController _controller;

        public void Start()
        {
            var enemyData = (_currentCombatState != null) ? _currentCombatState.Enemy : _enemyData;
            var model = new CombatModel(enemyData, _playerData);
            
            if (_view == null)
                throw new Exception("[CombatSystem] View is mandatory component - it's null");
            
            _controller = new CombatController(model, _view);
            _controller.OnEnable();
        }

        public void Update() => _controller.Update(Time.deltaTime);
        public void OnDisable() => _controller.OnDisable();
    }
}