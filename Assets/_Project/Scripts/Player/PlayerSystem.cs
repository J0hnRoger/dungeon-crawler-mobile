using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    private EventBinding<SkillLaunchedEvent> _skillLaunchedBinding;

    void OnEnable()
    {
        _skillLaunchedBinding = new EventBinding<SkillLaunchedEvent>(_controller.HandleSkillLaunched);
        EventBus<SkillLaunchedEvent>.Register(_skillLaunchedBinding);
    }

    void OnDisable()
    {
        EventBus<SkillLaunchedEvent>.Deregister(_skillLaunchedBinding);
    }


    private PlayerController _controller;

    void Awake()
    {
        _controller = new PlayerController();
    }

    void Update()
    {
        // Update animation based on movement
    }

}
