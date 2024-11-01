using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler.Skills;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public interface ICommand
    {
        void Execute();
    }
    
    public class SkillCommand : ICommand
    {
        private readonly SkillData data;
        public float cooldown => data.cooldown;

        public SkillCommand(SkillData data)
        {
            this.data = data;
        }
        
        public void Execute()
        {
            EventBus<SkillLaunchedEvent>.Raise(new SkillLaunchedEvent()
            {
                SkillName = data.name,
                AnimationName = data.animationName,
                Duration = data.cooldown
            });
        }
    }
}