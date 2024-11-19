using UnityEngine;

namespace DungeonCrawler
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ToggleInventoryUI()
        {
           _container.SetActive(!_container.activeSelf); 
        }
    }
}
