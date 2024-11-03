using System;
using _Project.Scripts.Common;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Events.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonCrawler._Project.Scripts.Common.InputManagement
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private InputActionAsset _allInputs;

        private InputAction _attackAction;
        private Camera _mainCamera;

        private EventBinding<SceneLoadedEvent> _sceneLoadedeventBinding;

        private void OnEnable()
        {
            // Update de la main camera
            _sceneLoadedeventBinding = new EventBinding<SceneLoadedEvent>(OnSceneLoaded);
            EventBus<SceneLoadedEvent>.Register(_sceneLoadedeventBinding);
        }

        public void OnDisable()
        {
            EventBus<SceneLoadedEvent>.Deregister(_sceneLoadedeventBinding);
        }

        private void OnSceneLoaded(SceneLoadedEvent obj)
        {
            _mainCamera = Camera.main;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _attackAction = _allInputs.FindActionMap("Gameplay")
                .FindAction("Attack");

            _attackAction.performed += OnTapPerformed;
            
            _allInputs.Enable();
        }

        private void OnTapPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            Vector2 screenPosition = Mouse.current.position.ReadValue();
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.isInProgress)
            {
                screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }

            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));

            EventBus<TapEvent>.Raise(new TapEvent() { WorldPosition = worldPosition, ScreenPosition = screenPosition });
        }
    }
}