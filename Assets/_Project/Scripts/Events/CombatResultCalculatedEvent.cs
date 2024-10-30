using System;
using System.Collections.Generic;
using _Project.Scripts.Common;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class CombatResultCalculatedEvent : IEvent
    {
        public bool IsLastCombat { get; set; }
        public bool Win { get; set; }
        public List<Reward> Rewards { get; set; }
    }

    [Serializable]
    public class Reward
    {
        private string Name { get; set; }
        private int Quantity { get; set; }
    }
}