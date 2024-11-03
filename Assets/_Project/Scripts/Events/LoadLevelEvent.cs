using System;
using _Project.Scripts.Common;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Events
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LevelPathAttribute : Attribute
    {
    }

    public class LoadLevelEvent : IEvent
    {
        [LevelPath]
        public string LevelPrefabPath { get; set; }
        
        public GameObject LevelPrefab { get; set; }
    }
}