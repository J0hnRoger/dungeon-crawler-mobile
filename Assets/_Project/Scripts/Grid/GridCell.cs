using System;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.Grid
{

public class GridCell : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public GridType GridType;
    [SerializeField]
    private GameObject outline;
        private GridCell[] gridList;
        public Action<GridCell> OnCellSelected { get; set; }

        public void OnEnable()
        {
            gridList = FindObjectsByType<GridCell>(FindObjectsSortMode.None);
            //gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnCellSelected?.Invoke(this);
            
            switch (GridType)
            {
                case GridType.Empty:
                        OutlinePosition();
                        EventBus<GridClickedEvent>.Raise(new GridClickedEvent()
                        {
                            Position = transform.position
                        });
                    break;
                case GridType.Enemy:
                    // TODO - Combat
                    Debug.Log("Combat avec un ennemi");
                    break;
                case GridType.Treasure:
                    // TODO - Coffre
                    Debug.Log("Ouverture d'un coffre au trésor");
                    break;
                case GridType.Boss:
                    // TODO - Boss
                    Debug.Log("Combat avec un boss");
                    break;
            }
    }

        public void OutlinePosition()
        {

            foreach (GridCell grid in gridList)
            {

                if (grid != this) grid.outline.SetActive(false);
                else grid.outline.SetActive(true);
            }
        }
}
}