using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.UI
{
    public class NotificationUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TMP_Text _notificationText;

        [SerializeField] private float _autoCloseDelay = 2f;
        [SerializeField] private float _fadeOutDuration = 1f;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private Coroutine _autoCloseCoroutine;

        public void Show(string message, float duration)
        {
            if (_autoCloseCoroutine != null)
            {
                StopCoroutine(_autoCloseCoroutine);
            }
            
            _canvasGroup.alpha = 1;
            _notificationText.text = message;

            gameObject.SetActive(true);
            _autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
           ClosePopup(); 
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
            gameObject.SetActive(false);
        }
    }
}