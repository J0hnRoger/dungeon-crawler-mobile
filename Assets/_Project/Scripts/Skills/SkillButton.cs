using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public class SkillButton : MonoBehaviour
    {
        public Image radialImage;
        public Image abilityIcon;
        public int index;
        public Key key;

        public event Action<int> OnButtonPressed = i => { };

        public void Initialize(int index, Key key)
        {
            this.key = key;
            this.index = index;
        }
        
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(index));
        }

        private void Update()
        {
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                OnButtonPressed(index);
            }
        }

        public void RegisterListener(Action<int> listener)
        {
            OnButtonPressed += listener;
        }
        
        public void UpdateButtonSprite(Sprite sprite)
        {
            abilityIcon.sprite = sprite; 
        }
        
        public void UpdateRadialFill(float progress)
        {
            if (radialImage)
                radialImage.fillAmount = progress; 
        }
        
        public void DeregisterListener(Action<int> listener)
        {
            OnButtonPressed -= listener;
        }
    }
}