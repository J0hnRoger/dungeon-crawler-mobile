using System.Collections.Generic;
using _Project.Scripts.Common.DependencyInjection;
using TMPro;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    public class EquipmentView : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private TMP_Text _equipmentTitle;
        [SerializeField] private GameObject _container;

        [SerializeField] private List<EquipmentSlot> EquipmentSlots = new ();
        
        [Provide]
        private EquipmentView ProvideEquipmentView()
        {
            return this;
        }
        
        public void ToggleUI()
        {
            SetUIEnable(!_container.gameObject.activeSelf);
        }
        
        private void SetUIEnable(bool show)
        {
           _equipmentTitle.gameObject.SetActive(show); 
           _container.SetActive(!_container.activeSelf); 
        }
    }
}