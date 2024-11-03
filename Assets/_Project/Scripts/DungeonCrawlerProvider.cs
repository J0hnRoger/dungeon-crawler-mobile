using System;
using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts
{
    /// <summary>
    /// This class is used to provide dependencies for the DungeonCrawler game.
    /// </summary>
    public class DungeonCrawlerProvider : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private SaveLoadSystem _saveLoadSystem;
        
        // Initialize all transverse services
        [Provide]
        public SceneLoader GetSceneLoader()
        {
            if (_sceneLoader == null)
                throw new Exception("SceneLoader not present in the bootstrap scene");

            return _sceneLoader;
        }
        
        [Provide]
        public SaveLoadSystem GetSaveLoadSystem()
        {
            if (_saveLoadSystem == null)
                throw new Exception("SaveLoadSystem not present in the bootstrap scene");

            return _saveLoadSystem;
        }
    }
}
