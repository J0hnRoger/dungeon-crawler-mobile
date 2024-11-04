using System;
using System.Linq;
using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Grid.Components;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridSystem : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;

        [SerializeField] private GridUIView _gridUI;
        [SerializeField] private GameObject _levelPrefab;

        private GameObject _currentLevel;

        private GridCell _startingPoint;
        private GridView _view;
        private GridController _controller;

        private EventBinding<LoadLevelEvent> _loadLevelBinding;

        private void OnEnable()
        {
            _loadLevelBinding = new EventBinding<LoadLevelEvent>(OnLoadLevel);
            EventBus<LoadLevelEvent>.Register(_loadLevelBinding);
        }

        private void Start()
        {
            // Initialize(_levelPrefab);
        }

        private void OnLoadLevel(LoadLevelEvent loadLevelEvent)
        {
            // Nettoyer l'ancien niveau si nécessaire
            if (_currentLevel != null)
                Destroy(_currentLevel);

            // Charger et instancier le nouveau niveau
            var levelPrefab = (loadLevelEvent.LevelPrefab != null) 
                ? loadLevelEvent.LevelPrefab 
                : Resources.Load<GameObject>(loadLevelEvent.LevelPrefabPath);
            
            Initialize(levelPrefab);
        }

        private void Initialize(GameObject levelPrefab)
        {
            _currentLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            
            _gridUI.SetLevelName(levelPrefab.name);
            
            // Setup du nouveau niveau
            _view = _currentLevel.GetComponentInChildren<GridView>();
            if (_view == null)
                throw new Exception("[GridSystem] GridView not found on level prefab");

            var startingPoint = _view.GetComponentsInChildren<GridCell>()
                .FirstOrDefault(cell => cell.IsStartingPoint);

            if (startingPoint == null)
                throw new Exception("[GridSystem] No starting point defined in level");

            var combatCells = _view.GetCellsOfType(GridType.Enemy);
            var bossCell = _view.GetCellsOfType(GridType.Boss);

            var model = new GridModel(startingPoint, combatCells.Concat(bossCell).ToList());
            _controller = new GridController(model, _view);
        }

        private void OnDisable() => _controller.OnDisable();
    }
}