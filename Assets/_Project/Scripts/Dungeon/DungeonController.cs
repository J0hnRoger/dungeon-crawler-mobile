using NUnit.Framework.Constraints;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public class DungeonController
    {

        public class Builder
        {
            private DungeonModel _model;
            
            public Builder WithDungeonGrid()
            {
                return this;
            }

            public DungeonController Build()
            {
                return new DungeonController();
            }
        }
    }
}