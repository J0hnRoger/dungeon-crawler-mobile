﻿using System;
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