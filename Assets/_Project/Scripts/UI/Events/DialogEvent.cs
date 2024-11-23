using _Project.Scripts.Common;
using _Project.Scripts.Common.EventBus;

namespace DungeonCrawler._Project.Scripts.UI.Events
{
    public class DialogEvent : IEvent
    {
        public string Message { get; private set; }
        public float Duration { get; private set; }
        public DialogType Type { get; private set; }

        // Factory methods pour plus de clarté à l'usage
        public static void ShowNotification(string message, float duration = 1f)
        {
            EventBus<DialogEvent>.Raise(new DialogEvent
            {
                Message = message, Duration = duration, Type = DialogType.Notification
            });
        }

        public static void ShowDialog(string message)
        {
            EventBus<DialogEvent>.Raise(new DialogEvent {Message = message, Type = DialogType.Dialog});
        }
    }
}