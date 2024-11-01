using System.Collections;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Dungeon
{
    public class DungeonResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultTitleTxt;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _close;

        [SerializeField] private float _autoCloseDelay = 3f;
        [SerializeField] private float _fadeOutDuration = 1.5f;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private EventBinding<CombatResultCalculatedEvent> _onCombatFinishedEventBinding;

        private Coroutine _autoCloseCoroutine;

        private void Awake()
        {
            _panel.SetActive(false);
            _close.onClick.AddListener(ClosePopup);
        }

        public void OnEnable()
        {
            _onCombatFinishedEventBinding = new EventBinding<CombatResultCalculatedEvent>(ShowCombatResult);
            EventBus<CombatResultCalculatedEvent>.Register(_onCombatFinishedEventBinding);
        }

        public void OnDisable()
        {
            EventBus<CombatResultCalculatedEvent>.Deregister(_onCombatFinishedEventBinding);
        }

        private void ShowCombatResult(CombatResultCalculatedEvent combatResultCalculatedEvent)
        {
            if (_autoCloseCoroutine != null)
            {
                StopCoroutine(_autoCloseCoroutine);
                _canvasGroup.alpha = 1;
            }
            _panel.SetActive(true);
            _resultTitleTxt.text = (combatResultCalculatedEvent.Win) ? "You WIN!" : "You Loose";
            _autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
        }

        private IEnumerator AutoCloseAfterDelay()
        {
            yield return new WaitForSeconds(_autoCloseDelay);

            float elapsedTime = 0;
            while (elapsedTime < _fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                _canvasGroup.alpha = 1 - (elapsedTime / _fadeOutDuration);
                yield return null;
            }
            ClosePopup();
        }

        private void ClosePopup()
        {
            _panel.SetActive(false);
        }
    }
}