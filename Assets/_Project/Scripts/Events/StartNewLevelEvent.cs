using _Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class StartNewLevelEvent : IEvent
    {
        public string LevelName { get; set; }
    }
}