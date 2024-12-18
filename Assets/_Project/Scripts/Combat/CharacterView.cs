﻿using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    /// <summary>
    /// Interact with a Character (player or enemy) (animation, sound, fx)
    /// </summary>
    public class CharacterView : MonoBehaviour
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
            // PlayAnimation(triggerName);
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