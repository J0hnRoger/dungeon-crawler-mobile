using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class CombatView : MonoBehaviour
    {
        [SerializeField] private Image EnemyHealth;
        [SerializeField] private Image PlayerHealth;

        public void UpdateEnemyHealth(int value)
        {
            EnemyHealth.fillAmount = value / 100f;
        }


        public void UpdatePlayerHealth(int value)
        {
            PlayerHealth.fillAmount = value / 100f;
        }
    }
}