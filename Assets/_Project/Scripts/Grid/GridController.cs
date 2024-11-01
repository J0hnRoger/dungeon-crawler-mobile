using System;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Grid.Components;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridController
    {
        private readonly GridModel _model;
        private readonly GridView _view;

        public GridController(GridModel model, GridView view)
        {
            _model = model;
            _view = view;

            ConnectView();
            ConnectModel();
        }

        private void ConnectView()
        {
            _view.OnCellSelected += UpdateCellState;
             
            _view.HideCells();
            _view.CellStart.gameObject.SetActive(true);
            // Update first cell
            UpdateCellState(_view.CellStart);
        }

        /// <summary>
        /// Déclenche l'action en fonction du type de case
        /// </summary>
        private void UpdateCellState(GridCell selected)
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
                    MoveOnCombatCell(selected);
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
            // Reveal neighbours
            RevealNeighbors(selected);
        }
        
        private void MoveOnCombatCell(GridCell selected)
        {
            if (selected.Enemy == null)
                throw new Exception("Combat Cell should have one enemy");
            
            EventBus<CombatStartedEvent>.Raise(new CombatStartedEvent() { 
                EnemyData = selected.Enemy,
                IsBoss = selected.GridType == GridType.Boss
            });
        }

        private void RevealNeighbors(GridCell selected)
        {
            var coordinates = _view.GetGridCoordinates(selected.transform.position);
            Vector3Int[] offsets = {
                new(1, 0, 0), // Droite
                new(-1, 0, 0), // Gauche
                new(0, 1, 0), // Haut
                new(0, -1, 0) // Bas
            };

            foreach (var offset in offsets)
            {
                Vector3Int neighborCoords = coordinates + offset;
                GridCell neighborCell = _view.GetGridCellAt(neighborCoords); 
                
                if (neighborCell != null)
                    neighborCell.Reveal();
            }
        }

        private void ConnectModel()
        {
        }
    }
}