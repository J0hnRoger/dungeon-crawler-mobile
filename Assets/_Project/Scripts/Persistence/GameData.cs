using System;

namespace _Project.Scripts.Persistence
{
    
    [Serializable]
    public class GameData
    {
        public string Name { get; set; }
        public string CurrentLevelName { get; set; }
    }
}