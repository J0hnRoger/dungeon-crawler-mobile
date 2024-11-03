using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Skills;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Events
{
    public struct SkillLaunchedEvent : IEvent
    {
        public string SkillName;
        public string AnimationName;
        public float Duration;
        /// <summary>
        /// Point d'impact du skill, en World Position 
        /// </summary>
        public HitInfo HitInfo;
        public float Timing { get; set; }
    }
}