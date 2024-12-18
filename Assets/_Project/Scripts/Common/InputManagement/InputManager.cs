﻿using System;
using System.Linq;
using _Project.Scripts.Common;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Events.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DungeonCrawler._Project.Scripts.Common.InputManagement
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private InputActionAsset _allInputs;

        private InputAction _attackAction;
        private Camera _mainCamera;

        private EventBinding<SceneLoadedEvent> _sceneLoadedeventBinding;
        private EventBinding<SceneUnloadedEvent> _sceneUnloadedeventBinding;

        private void OnEnable()
        {
            // Update de la main camera
            _sceneLoadedeventBinding = new EventBinding<SceneLoadedEvent>(OnSceneLoaded);
            EventBus<SceneLoadedEvent>.Register(_sceneLoadedeventBinding);
            
            _sceneUnloadedeventBinding = new EventBinding<SceneUnloadedEvent>(OnSceneUnloaded);
            EventBus<SceneUnloadedEvent>.Register(_sceneUnloadedeventBinding);
        }

        /// <summary>
        /// Action déclenchée à chaque "Destroy" de Singleton sur les scenes
        /// donc bien penser à ré-activer les Input dans le OnSceneLoaded et rattacher les events dans
        /// </summary>
        public void OnDisable()
        {
            // EventBus<SceneLoadedEvent>.Deregister(_sceneLoadedeventBinding);
            // _allInputs.Disable();
            // check if null, if disabled by Singleton baseclass
            // if (_attackAction != null)
            //     _attackAction.performed -= OnTapPerformed;
        }

        private void OnSceneLoaded(SceneLoadedEvent sceneLoadedEvent)
        {
            var loadedScene = sceneLoadedEvent.LoadedScene;
            _mainCamera = GetMainCamera(loadedScene);
            _allInputs.Enable();
        }
        
        private void OnSceneUnloaded(SceneUnloadedEvent sceneUnloadedEvent)
        {
            var loadedScene = sceneUnloadedEvent.UnloadedScene;
            _mainCamera = GetMainCamera(SceneManager.GetActiveScene());
            // on re-enable les inputs
            _allInputs.Enable();
        }

        private Camera GetMainCamera(Scene loadedScene)
        {
            var mainCameraInScene = loadedScene.GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<Camera>())
                .FirstOrDefault(cam => cam.CompareTag("MainCamera"));

            if (mainCameraInScene != null)
                return mainCameraInScene;

            var mainCamera = Camera.main; // pour du multi-scene additif
            if (mainCamera != null)
                Debug.Log($"Main Camera not found: use {mainCamera.name} instead.");
            else
                Debug.Log($"No camera on scene.");
            return mainCamera;
        }

        protected override void AwakeAsSingleton()
        {
            _mainCamera = Camera.main;

            _attackAction = _allInputs.FindActionMap("Gameplay")
                .FindAction("Attack");

            if (_attackAction == null)
                throw new Exception("[InputManager] Pas d'action 'Attack' trouvée dans le fichier Inputs");

            _attackAction.performed += OnTapPerformed;
        }

        private void OnTapPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            Vector2? screenPosition;
            if (Mouse.current != null)
                screenPosition = Mouse.current.position.ReadValue();
            else if (Touchscreen.current != null)
                screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            else
            {
                Debug.LogWarning("No mouse or touch input detected");
                return;
            }

            Vector3 worldPosition =
                _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.Value.x, screenPosition.Value.y, 0));

            EventBus<TapEvent>.Raise(new TapEvent()
            {
                WorldPosition = worldPosition, ScreenPosition = screenPosition.Value
            });
        }
    }
}