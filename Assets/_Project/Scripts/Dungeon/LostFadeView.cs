using System.Collections;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public class LostFadeView : MonoBehaviour
    {
        [SerializeField] private Image _fadeToBlackImage; // Image noire en UI
        [SerializeField] private float _lostFadeDuration = 3f;

        private EventBinding<CombatResultCalculatedEvent> _onCombatFinishedEventBinding;

        private Coroutine _fadeToBlackRoutine;

        public void OnEnable()
        {
            _onCombatFinishedEventBinding = new EventBinding<CombatResultCalculatedEvent>(FadeToBlack);
            EventBus<CombatResultCalculatedEvent>.Register(_onCombatFinishedEventBinding);
        }

        public void OnDisable()
        {
            EventBus<CombatResultCalculatedEvent>.Deregister(_onCombatFinishedEventBinding);
        }

        private void FadeToBlack(CombatResultCalculatedEvent combatResultCalculatedEvent)
        {
            if (combatResultCalculatedEvent.Win)
                return;
            
            if (_fadeToBlackRoutine != null)
                StopCoroutine(_fadeToBlackRoutine);

            Color c = _fadeToBlackImage.color;
            c.a = 0;
            _fadeToBlackImage.color = c;
             
            StartCoroutine(FadeToBlackCoroutine());
        }

        private IEnumerator FadeToBlackCoroutine()
        {
            Color c = _fadeToBlackImage.color;

            while (c.a < 1)
            {
                c.a += Time.deltaTime / _lostFadeDuration;
                _fadeToBlackImage.color = c;
                yield return null;
            }
        }
    }
}