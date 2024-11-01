using System;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridSystem : MonoBehaviour
    {
        [Inject] 
        private SceneLoader _sceneLoader;

        [SerializeField] private GridView _view;
        
        private GridController _controller;

        public void Start()
        {
            var model = new GridModel();
            if (_view == null)
                throw new Exception("[GridSystem] View cannot be null");
            
            _controller = new GridController(model, _view);
        }
    }
}