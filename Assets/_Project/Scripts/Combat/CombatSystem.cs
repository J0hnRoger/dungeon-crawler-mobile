using System;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Dungeon;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] private CombatView _view;

        [Inject] private CurrentCombat _currentCombat;
        
        [SerializeField] private EnemyData _enemyData;
        
        [SerializeField] private PlayerData _playerData;
        
        private CombatController _controller;

        public void Awake()
        {
            var enemyData = (_currentCombat != null) ? _currentCombat.Enemy : _enemyData;
            var model = new CombatModel(enemyData, _playerData);
            
            if (_view == null)
                throw new Exception("[CombatSystem] View is mandatory component - it's null");
            
            _controller = new CombatController(model, _view);
        }

        public void Update() => _controller.Update(Time.deltaTime);

        public void OnEnable() => _controller.OnEnable();
        public void OnDisable() => _controller.OnDisable();
    }
}