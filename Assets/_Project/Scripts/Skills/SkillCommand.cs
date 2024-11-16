using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler.Skills;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public interface ICommand
    {
        void Execute(float timing);
    }
    
    public class SkillCommand : ICommand
    {
        private readonly SkillData data;
        public float Cooldown => data.cooldown;
        public HitInfo Target;
        public float TimingPressed;

        public SkillCommand(SkillData data, HitInfo targetPoint, float timingPressed)
        {
            this.data = data;
            this.Target = targetPoint;
            this.TimingPressed = timingPressed;
        }
        
        public void Execute(float timing)
        {
            EventBus<SkillLaunchedEvent>.Raise(new SkillLaunchedEvent()
            {
                SkillName = data.name,
                HitInfo = Target,
                Timing = TimingPressed,
                AnimationName = data.animationName,
                Duration = data.cooldown
            });
        }
    }
}