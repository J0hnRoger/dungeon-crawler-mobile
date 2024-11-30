using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Common;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    public class TestEquipmentStore : MonoBehaviour, IEquipmentStore
    {
        [SerializeField] public List<EquipmentItem> _equipments;
       
        public ObservableList<EquipmentItem> Equipments { get; private set; } = new();

        public void Awake()
        {
            Equipments = new ObservableList<EquipmentItem>(_equipments);
        }
    }
}