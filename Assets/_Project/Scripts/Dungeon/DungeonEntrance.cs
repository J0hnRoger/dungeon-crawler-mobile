using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using UnityEngine.EventSystems;


public class DungeonEntrance : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string _levelPath;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Start new level: {this._levelPath}");
        EventBus<StartNewLevelEvent>.Raise(new StartNewLevelEvent()
        {
            LevelName = _levelPath
        });
    }
}
