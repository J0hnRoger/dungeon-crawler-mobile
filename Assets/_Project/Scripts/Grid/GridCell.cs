using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.Grid
{
    public class GridCell : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] public GridType GridType;
        [SerializeField] private GameObject outline;
        
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
    }
}