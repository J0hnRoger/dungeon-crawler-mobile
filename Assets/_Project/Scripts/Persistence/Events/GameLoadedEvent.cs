using _Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Persistence.Events
{
    public class GameLoadedEvent : IEvent
    {
        public string SaveName { get; set; }
        public GameData GameData { get;set; }
    }
}