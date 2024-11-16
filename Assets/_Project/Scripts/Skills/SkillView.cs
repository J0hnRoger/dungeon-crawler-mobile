using System;
using System.Collections.Generic;
using DungeonCrawler.Skills;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] private string _cooldownAnimationName = "SkillReady";
        [SerializeField] public string _directHitAnimationName = "SkillCleanHit";
        
        [SerializeField] public TimingVisualView _timingVisual;

        private void Awake()
        {
            _timingVisual.Initialize();
        }

        // TODO - sur le circle simple
        public void UpdateRadial(float progress)
        {
            if (float.IsNaN(progress))
                progress = 0;
            
            _timingVisual.UpdateRadialFill(progress);
        }

        public void StartCooldownAnimation()
        {
            _timingVisual.TriggerAnimation(_cooldownAnimationName);
        }
        
        public void LaunchCleanHitAnimation()
        {
            _timingVisual.TriggerAnimation(_directHitAnimationName);
        }
    }
}