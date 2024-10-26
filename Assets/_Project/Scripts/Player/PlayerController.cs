using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Player
{
    public class PlayerController
    {
        internal void HandleSkillLaunched(SkillLaunchedEvent @event)
        {
            Debug.Log($"Player Controller received event and start animation: {@event.AnimationName}");
        }
    }
}
