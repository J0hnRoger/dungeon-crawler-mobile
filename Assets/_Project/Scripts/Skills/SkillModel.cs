using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Skills;

namespace DungeonCrawler.Skills
{
    public class SkillModel
    {
        public readonly ObservableList<Skill> _skills = new();

        public void Add(Skill s)
        {
           _skills.Add(s); 
        }
    }

    public class Skill
    {
        public readonly SkillData Data;

        public Skill(SkillData data)
        {
            this.Data = data;
        }

        public SkillCommand CreateCommand()
        {
            return new SkillCommand(Data);
        }
    }
}