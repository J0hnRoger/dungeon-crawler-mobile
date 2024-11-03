using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler.Skills;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public interface ICommand
    {
        void Execute();
    }
    
    public class SkillCommand : ICommand
    {
        private readonly SkillData data;
        public float Cooldown => data.cooldown;
        public HitInfo Target;
        public float Timing;

        public SkillCommand(SkillData data, HitInfo targetPoint, float timing)
        {
            this.data = data;
            this.Target = targetPoint;
            this.Timing = timing;
        }
        
        public void Execute()
        {
            EventBus<SkillLaunchedEvent>.Raise(new SkillLaunchedEvent()
            {
                SkillName = data.name,
                HitInfo = Target,
                Timing = Timing,
                AnimationName = data.animationName,
                Duration = data.cooldown
            });
        }
    }
}