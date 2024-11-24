using DungeonCrawler._Project.Scripts.Inventory;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    public enum BodyPart
    {
        HEAD,
        CHEST,
        LEFT,
        RIGHT,
        LEGS
    }
    
    public class EquipmentSlot : MonoBehaviour, IDropHandler
    {
        [CanBeNull] public DungeonItem _currentItem = null;
        
        [SerializeField] private Image _icon;
        [SerializeField] private BodyPart _bodyPart;
        
        public void SetItem(DungeonItem item)
        {
            _currentItem = item;
            _icon.sprite = item.Data.Icon;
            _icon.enabled = true;
        }

        public void EmptySlot()
        {
            _currentItem = null;
            _icon.enabled = false;
        }

        public bool CanEquipItem(DungeonItem item)
        {
            // TODO - controler le type Equipment et la bonne partie du corps
            return true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var sourceSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
            if (sourceSlot == null || sourceSlot._currentItem == null) return;

            if (CanEquipItem(sourceSlot._currentItem))
            {
                if (_currentItem != null && _currentItem.Data != null)
                {
                    var tempItem = _currentItem;
                    SetItem(sourceSlot._currentItem);
                    sourceSlot.SetItem(tempItem);
                }
                else
                {
                    SetItem(sourceSlot._currentItem);
                    sourceSlot.EmptySlot();
                }
            }
        }
    }
}