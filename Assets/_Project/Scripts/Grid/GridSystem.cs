using System.Linq;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridSystem : MonoBehaviour
    {
        private UnityEngine.Grid _grid;
        private GridCell[] _gridCells;

        public void Awake()
        {
            _grid = GetComponent<UnityEngine.Grid>();
            // On stock des references à tous les GO enfants qui portent le script GridCell 
            _gridCells = GetComponentsInChildren<GridCell>();

            foreach (GridCell gridCell in _gridCells)
            {
                gridCell.OnCellSelected += CheckGrid;
            }
        }

        public void CheckGrid(GridCell selected)
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
                    // TODO - retirer le FOG au-dessus, ou changer d'apparence pour passer d'une case "mystère" à une case du jeu
                    // neighborCell.Reveal() <-- méthode custom;
                }
            }
        }
    }
}