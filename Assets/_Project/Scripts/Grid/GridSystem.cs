using System.Linq;
using System.Threading.Tasks;
using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridSystem : MonoBehaviour
    {
        private UnityEngine.Grid _grid;
        private GridCell[] _gridCells;
        
        [Inject] 
        private SceneLoader _sceneLoader;
        
        [SerializeField] private GridCell _cellStart;

        public void Awake()
        {
            _grid = GetComponent<UnityEngine.Grid>();
            // On stock des references à tous les GO enfants qui portent le script GridCell 
            _gridCells = GetComponentsInChildren<GridCell>();

            foreach (GridCell gridCell in _gridCells)
            {
                gridCell.OnCellSelected += UpdateCells;
                gridCell.gameObject.SetActive(false);
            }

            InitializeFirstCell();
        }

        public void InitializeFirstCell()
        {
            _cellStart.gameObject.SetActive(true);
            UpdateCells(_cellStart);
        }

        /// <summary>
        /// Déclenche l'action en fonction du type de case
        /// </summary>
        private void UpdateCells(GridCell selected)
        {
            switch (selected.GridType)
            {
                case GridType.Empty:
                    MoveOnGridEmptyCell(selected);
                    break;
                case GridType.Enemy:
                    MoveOnCombatCell(selected);
                    break;
                case GridType.Treasure:
                    // TODO - Coffre
                    MoveOnTreasureCell(selected);
                    break;
                case GridType.Boss:
                    // TODO - Boss
                    Debug.Log("Combat avec un boss");
                    break;
            }
        }

        private void MoveOnTreasureCell(GridCell selected)
        {
            Debug.Log("Move on Treasure cell");
        }

        private void MoveOnGridEmptyCell(GridCell selected)
        {
            EventBus<EmptyCellClickedEvent>.Raise(new EmptyCellClickedEvent() { Position = selected.transform.position });
            selected.SetOutline(true);
            // Cell Outline 
            _gridCells.ToList().ForEach(cell => cell.SetOutline(false));
            // Reveal neighbours
            UpdateNextCells(selected);
        }
        
        private void MoveOnCombatCell(GridCell selected)
        {
            EventBus<CombatStartedEvent>.Raise(new CombatStartedEvent() { });
        }

        private void UpdateNextCells(GridCell selected)
        {
            var coordinates = _grid.WorldToCell(selected.transform.position);
            Debug.Log(coordinates);
            Vector3Int[] offsets =
            {
                new Vector3Int(1, 0, 0), // Droite
                new Vector3Int(-1, 0, 0), // Gauche
                new Vector3Int(0, 1, 0), // Haut
                new Vector3Int(0, -1, 0) // Bas
            };

            foreach (var offset in offsets)
            {
                Vector3Int neighborCoords = coordinates + offset;
                GridCell neighborCell = _gridCells.FirstOrDefault(cell =>
                    _grid.WorldToCell(cell.transform.position) == neighborCoords);

                if (neighborCell != null)
                {
                    Debug.Log($"Neighbor found at: {neighborCoords}");
                    neighborCell.Reveal();
                }
            }
        }
    }
}