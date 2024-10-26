using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.Grid
{

public class GridCell : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public GridType GridType;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (GridType)
        {
            case GridType.Empty:
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
}
}