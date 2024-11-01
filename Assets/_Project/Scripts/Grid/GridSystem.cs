using System;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Common.ScriptableObjects;
using DungeonCrawler._Project.Scripts.Grid.Components;
using DungeonCrawler._Project.Scripts.Grid.SO;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridSystem : MonoBehaviour
    {
        [Inject] 
        private SceneLoader _sceneLoader;

        [Header("Configuration")]
        
        [SerializeField, InlineInspector] 
        private GridConfig _config;

        [SerializeField] private GridCell _startingPoint;
        
        [SerializeField] private GridView _view;
        
        private GridController _controller;

        public void Start()
        {
            var model = new GridModel(_startingPoint);
            if (_view == null)
                throw new Exception("[GridSystem] View cannot be null");
            
            _controller = new GridController(model, _view);
        }
    }
}