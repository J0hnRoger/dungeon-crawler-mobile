using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Inventory;

namespace DungeonCrawler._Project.Scripts.Persistence
{
    [Serializable]
    public class GameData
    {
        public string Name;
        public List<LevelProgression> LevelProgressions = new();
        public List<DungeonItem> Inventory = new();
        public List<DungeonItem> Equipments = new();
        
        public void UpdateProgression(string levelName)
        {
            var levelProgression = LevelProgressions
                .Where(lp => lp.LevelName == levelName);
            
            if (levelProgression.Any())
            {
                levelProgression.First().NbRuns++;
            }
            else
            {
                LevelProgressions.Add(new LevelProgression() {LevelName = levelName, IsActive = true, NbRuns = 0});
            }
        }
    }

    [Serializable]
    public class LevelProgression
    {
        public string LevelName;
        public int NbRuns;
        public bool IsActive;
    }
}