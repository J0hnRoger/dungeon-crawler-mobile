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


        public void SetEnemyName(string enemyName)
        {
            _enemyName.text = enemyName;
        }
        
        public void UpdateEnemyHealth(int value)
        {
            _enemyHealth.fillAmount = value / 100f;
        }


        public void UpdatePlayerHealth(int value)
        {
            _playerHealth.fillAmount = value / 100f;
        }
    }
}