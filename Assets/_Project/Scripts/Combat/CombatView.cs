using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatView : MonoBehaviour
    {
        [SerializeField] private Image _enemyHealth;
        [SerializeField] private Image _playerHealth;
        [SerializeField] private TMP_Text _enemyName;
        [SerializeField] private TMP_Text _enemyLevel;
        [SerializeField] private Image _cdGauge;

        [SerializeField] private CharacterView _playerView;
        [SerializeField] private CharacterView _enemyView;
        
        /// <summary>
        /// Object instancié à la position de l'attack
        /// </summary>
        [FormerlySerializedAs("_attackIndicator")] [SerializeField] private GameObject _attackIndicatorPrefab;
        
        public void SetEnemyName(string enemyName)
        {
            _enemyName.text = enemyName;
        }

        public void SetEnemyLevel(string enemyLevel)
        {
            _enemyLevel.text = enemyLevel;
        }

        public void UpdateCooldown(float progress)
        {
            if (float.IsNaN(progress))
                progress = 0;
            
            _cdGauge.fillAmount = progress; 
        }
        
        public void UpdateEnemyHealth(float percent)
        {
            _enemyHealth.fillAmount = percent;
        }

        public void UpdatePlayerHealth(float percent)
        {
            _playerHealth.fillAmount = percent;
        }

        public void EnemyAttack(string animationName)
        {
           _enemyView.Attack(animationName); 
        }
        
        public void PlayerAttack(string animationName)
        {
           _playerView.Attack(animationName); 
        }

        public void ShowAttackImpact(Vector3 targetPosition)
        {
            GameObject particleInstance = Instantiate(_attackIndicatorPrefab, targetPosition, Quaternion.identity);
            Destroy(particleInstance, 2f); // Destroy the effect after a short duration    
        }

        public void ShowCriticalHitFeedback()
        {
            EventBus<DirectHitEvent>.Raise(new DirectHitEvent());
        }
    }
}