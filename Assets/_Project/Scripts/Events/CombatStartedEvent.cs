﻿using _Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Combat.SO;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class CombatStartedEvent : IEvent
    {
        public EnemyData EnemyData;
        public bool IsBoss;
    }
}