using System;

namespace DungeonCrawler._Project.Scripts.Grid
{
    [Serializable]
    public enum GridType
    {
        Empty,
        Enemy,
        Treasure,
        Boss
    }
}