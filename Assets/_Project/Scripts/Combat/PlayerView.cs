using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    /// <summary>
    /// Interact with Player (animation, sound, fx) when attack is trigger
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animationName;

        public void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        /// <summary>
        /// Called each attack during a combat 
        /// </summary>
        /// <param name="triggerName"></param>
        public void Attack(string triggerName)
        {
            Debug.Log($"Player attack with {triggerName}");
            PlayAnimation(triggerName);
        }

        private void PlayAnimation(string triggerName)
        {
            if (_animator == null)
                Debug.Log($"Animation {triggerName} déclenchée - pas d'animator");
            else
                _animator.SetTrigger(triggerName);
        }
    }
}