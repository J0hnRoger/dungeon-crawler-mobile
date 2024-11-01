using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Grid.Components;
using JetBrains.Annotations;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridModel
    {
        public readonly GridCell CellStart;
        
        [CanBeNull] public GridCell ActiveCombatCell;
        
        public List<GridCell> Combats;

        public GridModel(GridCell cellStart, List<GridCell> combatCells)
        {
            CellStart = cellStart;
            Combats = combatCells;
        }

        public void ResetActiveCombatCell()
        {
            ActiveCombatCell = null;
        }
    }
}