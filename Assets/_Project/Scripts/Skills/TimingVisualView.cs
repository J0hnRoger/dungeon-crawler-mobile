using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public class TimingVisualView : MonoBehaviour
    {
        public Image radialImage;
        public int index;

        [SerializeField] private Animator _animator;

        public void Initialize()
        { }
        
        public void UpdateRadialFill(float progress)
        {
            if (radialImage)
                radialImage.fillAmount = progress; 
        }

        public void TriggerAnimation(string animationName)
        {
            //_animator.SetTrigger(animationName);
            _animator.Play(animationName);
        }
    }
}