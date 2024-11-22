using _Project.Scripts.Common;
using _Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.Persistence;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class GameLoadedEvent : IEvent
    {
        public string SaveName { get; set; }
        public GameData GameData { get;set; }
    }
}