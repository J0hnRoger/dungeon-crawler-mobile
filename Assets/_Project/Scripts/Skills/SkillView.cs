using System;
using System.Collections.Generic;
using DungeonCrawler.Skills;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] public SkillButton[] buttons;

        // Qwerty keyboard by default
        private readonly Key[] _keys = { Key.Q, Key.W, Key.E, Key.R };

        private void Awake()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i >= _keys.Length)
                {
                    Debug.LogError("Not enough keycodes");
                    break;
                }
                
                buttons[i].Initialize(i, _keys[i]);
            }
        }

        public void UpdateRadial(float progress)
        {
            if (float.IsNaN(progress))
                progress = 0;
            
            Array.ForEach(buttons, button => button.UpdateRadialFill(progress));
        }

        public void UpdateButtonSprites(IList<Skill> skills)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < skills.Count)
                {
                    buttons[i].UpdateButtonSprite(skills[i].data.icon);
                }
                else
                {
                   buttons[i].gameObject.SetActive(false); 
                }
            }
        }
    }
}