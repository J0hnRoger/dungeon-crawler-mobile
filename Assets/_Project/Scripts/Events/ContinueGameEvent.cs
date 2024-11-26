using _Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class ContinueGameEvent : IEvent
    {
        public string SaveName { get;set; } 
    }
}