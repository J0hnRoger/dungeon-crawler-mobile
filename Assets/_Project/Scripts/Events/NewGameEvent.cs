using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Persistence;

namespace DungeonCrawler._Project.Scripts
{
    public class NewGameEvent : IEvent
    {
        public GameData GameData;
    }
}