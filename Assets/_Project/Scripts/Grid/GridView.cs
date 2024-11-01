using System;
using System.Linq;
using DungeonCrawler._Project.Scripts.Grid.Components;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridView : MonoBehaviour
    {
        private UnityEngine.Grid _grid;
        private GridCell[] _gridCells;

        public Action<GridCell> OnCellSelected = cell => {};
        
        [SerializeField] public GridCell CellStart;

        public void Awake()
        {
            _grid = GetComponent<UnityEngine.Grid>();
            // On stock des references à tous les GO enfants qui portent le script GridCell 
            _gridCells = GetComponentsInChildren<GridCell>();
            foreach (GridCell gridCell in _gridCells)
            {
                gridCell.OnCellSelected += HandleCellSelected;
            }
        }
        
        private void HandleCellSelected (GridCell selected)
        {
            foreach (GridCell gridCell in _gridCells)
                gridCell.SetOutline(false);
            selected.SetOutline(true);
            OnCellSelected?.Invoke(selected);
        }
        
        public void HideCells()
        {
            foreach (GridCell gridCell in _gridCells)
            {
                gridCell.SetOutline(false);
                gridCell.gameObject.SetActive(false);
            }
        }

        public Vector3Int GetGridCoordinates(Vector3 worldPosition)
        {
           return _grid.WorldToCell(worldPosition);
        }

        public GridCell GetGridCellAt(Vector3Int gridPosition)
        {
            return _gridCells.FirstOrDefault(cell =>
                    _grid.WorldToCell(cell.transform.position) == gridPosition);
        }
    }
}