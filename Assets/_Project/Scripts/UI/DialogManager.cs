using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.UI.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.UI
{
    /// <summary>
    /// Service chargé de la gestion des UI
    /// basé sur les Events 
    /// </summary>
    public class DialogService : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private NotificationUI _notificationPrefab;

        private EventBinding<DialogEvent> _dialogEventBinding;

        private void OnEnable()
        {
            _dialogEventBinding = new EventBinding<DialogEvent>(HandleDialogEvent);
            EventBus<DialogEvent>.Register(_dialogEventBinding);
        }

        private void OnDisable()
        {
            EventBus<DialogEvent>.Deregister(_dialogEventBinding);
        }

        private void HandleDialogEvent(DialogEvent dialogEvent)
        {
            switch (dialogEvent.Type)
            {
                case DialogType.Notification:
                    _notificationPrefab.Show(dialogEvent.Message, dialogEvent.Duration);
                    break;
                case DialogType.Dialog:
                    // _dialogPrefab.Show(dialogEvent.Message);
                    break;
            }
        }
    }
}