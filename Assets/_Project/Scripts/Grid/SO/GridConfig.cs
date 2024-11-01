using DungeonCrawler._Project.Scripts.Grid.Components;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid.SO
{
    [CreateAssetMenu(fileName = "New Grid Config", menuName = "ScriptableObjects/GridConfig")]
    public class GridConfig : ScriptableObject
    {
        public GridCell CellStart;
    }
}