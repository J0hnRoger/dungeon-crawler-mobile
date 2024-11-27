﻿using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler._Project.Scripts.Equipment;
using UnityEngine.Serialization;

namespace DungeonCrawler._Project.Scripts.Inventory
{
    public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [CanBeNull] public DungeonItem CurrentItem;

        public Action<ItemSlot> OnDropAccepted;
        
        public int Index;
        public int quantity;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _quantityTxt;

        private GameObject _draggedIcon;
        private RectTransform _draggedRect;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        public void SetItem(DungeonItem item)
        {
            CurrentItem = item;
            _icon.sprite = item.Data.Icon;
            _quantityTxt.enabled = true;
            _quantityTxt.text = item.Quantity.ToString();
            _icon.enabled = true;
        }

        public void EmptySlot()
        {
            CurrentItem = null;
            _quantityTxt.enabled = false;
            _icon.sprite = null;
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

            var equipmentSlot = results
                .Select(r => r.gameObject.GetComponent<EquipmentSlot>())
                .FirstOrDefault(s => s != null);

            if (equipmentSlot != null && equipmentSlot.CanEquipItem(CurrentItem))
            {
                OnDropAccepted?.Invoke(this);
            }
        }
    }
}