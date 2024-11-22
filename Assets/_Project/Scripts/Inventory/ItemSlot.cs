using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class ItemSlot : MonoBehaviour
    {
        [CanBeNull] public DungeonItem _currentItem;
        
        public int Index;
        public int quantity;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _quantityTxt;

        public void SetItem(DungeonItem item)
        {
            _currentItem = item;
            _icon.sprite = item.Data.Icon;
            _quantityTxt.text = item.Quantity.ToString();
            _icon.enabled = true;
        }

        public void EmptySlot()
        {
            _currentItem = null;
            _icon.enabled = false;
        }
    }
}