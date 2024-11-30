using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Inventory;
using DungeonCrawler._Project.Scripts.UI.Events;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.Equipment
{
    public class EquipmentSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [CanBeNull] public EquipmentItem CurrentItem = null;

        [SerializeField] private Image _icon;
        [SerializeField] private BodyPart _bodyPart;

        public Action<EquipmentItem> OnEquip;
        public Action<EquipmentItem> OnUnequip;
        
        private GameObject _draggedIcon;
        private RectTransform _draggedRect;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        public void SetItem(EquipmentItem item)
        {
            CurrentItem = item;
            _icon.sprite = item.Data.Icon;
            _icon.enabled = true;
        }

        public void EmptySlot()
        {
            CurrentItem = null;
            _icon.sprite = null;
        }

        public bool CanEquipItem(DungeonItem item)
        {
            if (!(item is EquipmentItem))
            {
                DialogEvent.ShowNotification($"Item {item.Data.Name} is not an equipment");
                return false;
            }
            
            if (CurrentItem != null && CurrentItem.Data != null && CurrentItem != item)
                return false;
            return true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var sourceSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
            if (sourceSlot == null || sourceSlot.CurrentItem == null) return;

            if (CanEquipItem(sourceSlot.CurrentItem))
            {
                var equipmentItem = sourceSlot.CurrentItem as EquipmentItem;
                SetItem(equipmentItem);
                OnEquip?.Invoke(equipmentItem);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (CurrentItem == null || CurrentItem.Data == null) return;

            // Créer une copie temporaire de l'icône
            _draggedIcon = new GameObject("Dragged Icon");
            _draggedIcon.transform.SetParent(_canvas.transform);
            _draggedRect = _draggedIcon.AddComponent<RectTransform>();
            Image draggedImage = _draggedIcon.AddComponent<Image>();
            
            // Copier les propriétés de l'image originale
            draggedImage.sprite = _icon.sprite;
            draggedImage.raycastTarget = false;
            
            // Configurer la taille et la position
            _draggedRect.sizeDelta = _icon.rectTransform.sizeDelta;
            _draggedRect.position = _icon.rectTransform.position;
            
            // Rendre l'icône originale semi-transparente
            _icon.color = new Color(1, 1, 1, 0.5f);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedRect == null) return;
            _draggedRect.position = eventData.position;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (CurrentItem == null || CurrentItem.Data == null) return;

            // Restaurer l'opacité de l'icône originale
            _icon.color = Color.white;
            
            // Détruire l'icône temporaire
            if (_draggedIcon != null)
                Destroy(_draggedIcon);

            // Vérifier si on est au-dessus d'un slot d'équipement
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            var itemSlot = results
                .Select(r => r.gameObject.GetComponent<ItemSlot>())
                .FirstOrDefault(s => s != null);

            if (itemSlot != null && itemSlot.CanPickUpItem(CurrentItem))
            {
                OnUnequip?.Invoke(CurrentItem);
                EmptySlot();
            }
        }
    }
}