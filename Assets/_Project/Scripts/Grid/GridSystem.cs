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

        private void OnLoadLevel(LoadLevelEvent loadLevelEvent)
        {
            // Nettoyer l'ancien niveau si nécessaire
            if (_currentLevel != null)
                Destroy(_currentLevel);

            // Charger et instancier le nouveau niveau
            var levelPrefab = Resources.Load<GameObject>(loadLevelEvent.LevelPrefabPath);
            _currentLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

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