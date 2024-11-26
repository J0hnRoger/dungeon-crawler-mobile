using System;
using DungeonCrawler._Project.Scripts.Common;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Inventory.SO
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item")]
    [SerializableSO]
    public class DungeonItemSO : ScriptableObject
    {
        [SerializeField, ReadOnly]
        public string Guid; 
        
        public string Name;
        public Sprite Icon;
        public GameObject ItemPrefab;

       private void OnValidate()
       {
           // Génère un nouveau GUID uniquement s'il n'a pas déja ete généré
           if (String.IsNullOrEmpty(Guid))
           {
               Guid = System.Guid.NewGuid().ToString(); 
               EditorUtility.SetDirty(this);
           }
       }
    }
}