using UnityEngine;

namespace DungeonCrawler.Skills
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/Skills")]
    public class SkillData : ScriptableObject
    {
        public string skilName;
        public AnimationClip animationClip;
        public float cooldown;
        public Sprite icon;
    }
}