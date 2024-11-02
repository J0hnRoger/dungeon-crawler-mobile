using System;
using System.Linq;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Grid.Components;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridSystem : MonoBehaviour
    {
        [Inject] 
        private SceneLoader _sceneLoader;

        [SerializeField] private GridCell _startingPoint;
        
        [SerializeField] private GridView _view;
        
        private GridController _controller;

        public void Start()
        {
            var combatCells = _view.GetCellsOfType(GridType.Enemy);
            var bossCell = _view.GetCellsOfType(GridType.Boss);
            
            var model = new GridModel(_startingPoint, combatCells.Concat(bossCell).ToList());
            
            if (_view == null)
                throw new Exception("[GridSystem] View cannot be null");
            
            _controller = new GridController(model, _view);
        }

        private void OnDisable() => _controller.OnDisable();
    }
}