using _Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Events
{
    public struct SkillLaunchedEvent : IEvent
    {
        public string SkillName;
        public string AnimationName;
        public float Duration;
    }
}