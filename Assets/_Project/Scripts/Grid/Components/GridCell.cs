using System;
using DungeonCrawler._Project.Scripts.Combat.SO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.Grid.Components
{
    public class GridCell : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] public GridType GridType;
        [SerializeField] private GameObject outline;
        [SerializeField] public EnemyData Enemy;
        [SerializeField] public bool IsStartingPoint;
        
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _completedMaterial;

        [SerializeField] public bool Active = true;

        [HideInInspector] public Vector3Int Coordinates;
        
        public Action<GridCell> OnCellSelected { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnCellSelected?.Invoke(this);
        }

        public void SetOutline(bool isActive)
        {
            if (outline == null)
                return;

            outline.SetActive(@isActive);
        }

        public void Reveal()
        {
            gameObject.SetActive(true);
        }

        public void Complete()
        {
            Active = false;
            if (_meshRenderer != null)
                _meshRenderer.material = _completedMaterial;
        }
    }
}