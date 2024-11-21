using _Project.Scripts.Common.DependencyInjection;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class InventoryView : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        private InventoryView provideInventoryView()
        {
            return this;
        } 
        
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
