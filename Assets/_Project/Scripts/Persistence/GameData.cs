using System;
using System.Collections.Generic;

namespace _Project.Scripts.Persistence
{
    [Serializable]
    public class GameData
    {
        public string Name;
        public List<LevelProgression> LevelProgressions = new();
    }

    [Serializable]
    public class LevelProgression
    {
        public string LevelName;
        public int NbRuns;
        public bool IsActive;
    }
}