using DungeonCrawler._Project.Scripts.Skills;
using UnityEngine;

namespace DungeonCrawler.Skills
{
    // Entry Point
    public class SkillSystem : MonoBehaviour
    {
        [SerializeField] private SkillView _view;
        [SerializeField] private SkillData[] _skills;

        SkillController _controller;

        void Awake()
        {
            _controller = new SkillController.Builder()
                .WithSkills(_skills)
                .Build(_view);
        }

        void Update() => _controller.Update(Time.deltaTime);
        
    }
}
