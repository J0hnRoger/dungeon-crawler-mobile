using System;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

public class PlayerController
{
    internal void HandleSkillLaunched(SkillLaunchedEvent @event)
    {
        Debug.Log($"Player Controller received event and start animation: {@event.AnimationName}");
    }
}
