using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    [Serializable]
    public class Equipment
    {
        public  SkinnedMeshRenderer BodySocket;
        public Mesh Mesh;
        public Material Material;
    }
    
    public class EquipmentSwitch : MonoBehaviour
    {
        public List<Equipment> Equipments;
        
        private GameObject currentEquipment;

        public void EquipRandom()
        {
            if (currentEquipment != null)
                Destroy(currentEquipment);
            
            int randomIndex = Random.Range(0, Equipments.Count);
            var equipment = Equipments[randomIndex]; 
            equipment.BodySocket.sharedMesh = equipment.Mesh;

            // Applique le Material, si défini
            if (equipment.Material != null)
                equipment.BodySocket.material = equipment.Material;
        }

        public void Unequip()
        {
            if (Equipments == null || Equipments.Count == 0)
            {
                Debug.LogWarning("No equipment to unequip.");
                return;
            }

            foreach (var equipment in Equipments)
            {
                if (equipment.BodySocket != null)
                {
                    equipment.BodySocket.sharedMesh = null; // Retire le Mesh
                    equipment.BodySocket.material = null; // Retire le Material
                }
            }

            Debug.Log("All equipment unequipped.");
        }
    }
}