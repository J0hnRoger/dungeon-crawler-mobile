using System;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] private CombatView _view;
        
        private CombatController _controller;

        public void Awake()
        {
            var model = new CombatModel();
            model.Enemy = new Enemy("Test enemy", 10, 5);
            model.Player = new Player("Test player", 20, 5);

            if (_view == null)
                throw new Exception("[CombatSystem] View is mandatory component - it's null");
            
            _controller = new CombatController(model, _view);
        }

        public void OnEnable() => _controller.OnEnable();
        public void OnDisable() => _controller.OnDisable();
        
    }
}