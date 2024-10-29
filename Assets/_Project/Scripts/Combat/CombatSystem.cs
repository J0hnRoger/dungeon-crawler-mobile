using System;
using DungeonCrawler._Project.Scripts.Combat.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] private CombatView _view;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private PlayerData _playerData;
        
        private CombatController _controller;

        public void Awake()
        {
            var model = new CombatModel(_enemyData, _playerData);
            
            if (_view == null)
                throw new Exception("[CombatSystem] View is mandatory component - it's null");
            
            _controller = new CombatController(model, _view);
        }

        public void OnEnable() => _controller.OnEnable();
        public void OnDisable() => _controller.OnDisable();
    }
}