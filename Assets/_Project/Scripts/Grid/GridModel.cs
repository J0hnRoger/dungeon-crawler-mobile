using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Combat.SO;
using DungeonCrawler._Project.Scripts.Grid.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridModel
    {
        public readonly GridCell CellStart;

        public bool DungeonClear => Ennemies.All(e => e.IsDefeated);     
            
        [CanBeNull] public GridCell ActiveCombatCell;
        
        public List<EnemyModel> Ennemies;

        public GridModel(GridCell cellStart, List<GridCell> ennemyCells)
        {
            CellStart = cellStart;
            Ennemies = ennemyCells.Select(e => 
                    new EnemyModel(e.Coordinates, e.Enemy, e.Active))
                .ToList();
        }

        public void FinishCurrentCombat()
        {
            if (ActiveCombatCell == null)
                throw new Exception("Aucun combat en cours");
            var enemy = Ennemies.First(e => e.Coordinates == ActiveCombatCell.Coordinates);
            enemy.IsDefeated = true;
             
            ActiveCombatCell = null;
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