using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatView : MonoBehaviour
    {
        [SerializeField] private Image _enemyHealth;
        [SerializeField] private Image _playerHealth;
        [SerializeField] private TMP_Text _enemyName;
        [SerializeField] private Image _cdGauge;

        [SerializeField] private PlayerView _playerView;
        
        public void SetEnemyName(string enemyName)
        {
            _enemyName.text = enemyName;
        }
        
        public void UpdateRadial(float progress)
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

        public void PlayerAttack(string animationName)
        {
           _playerView.Attack(animationName); 
        }
    }
}