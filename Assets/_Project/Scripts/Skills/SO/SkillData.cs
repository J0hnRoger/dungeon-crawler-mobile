using UnityEngine;

namespace DungeonCrawler.Skills
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/Skills")]
    public class SkillData : ScriptableObject
    {
        public string Name;
        public AnimationClip animationClip;
        public float duration;
        public Sprite icon;

        private void OnValidate()
        {
        }
    }
}