using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Grid.Components;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridModel
    {
        public readonly Vector3Int StartPosition;

        public bool DungeonClear => Ennemies.All(e => e.IsDefeated);     
            
        public Vector3Int? CurrentCombatPosition { get; private set; }
        
        public List<EnemyModel> Ennemies;

        public GridModel(GridCell startPosition, List<GridCell> ennemyCells)
        {
            StartPosition = startPosition.Coordinates;
            Ennemies = ennemyCells.Select(e => 
                    new EnemyModel(e.Coordinates, e.Enemy, e.Active))
                .ToList();
        }

        public void StartCombat(GridCell currentCombat)
        {
            CurrentCombatPosition = currentCombat.Coordinates;
        }
        
        public void FinishCurrentCombat(bool win)
        {
            if (CurrentCombatPosition == null)
                throw new Exception("Aucun combat en cours");
            var enemy = Ennemies.First(e => e.Coordinates == CurrentCombatPosition);
            enemy.IsDefeated = win;
             
            CurrentCombatPosition = null;
        }
    }

    /// <summary>
    /// Représente un enemy sur la Grid
    /// </summary>
    public class EnemyModel
    {
        public EnemyData Data { get; private set; }
        public Vector3Int Coordinates { get; private set; }
        public bool IsDefeated { get; set; } = false;
        
        public EnemyModel(Vector3Int positionOnGrid, EnemyData data, bool active)
        {
            Coordinates = positionOnGrid;
            IsDefeated = !active;
            Data = data;
        }
    }
}