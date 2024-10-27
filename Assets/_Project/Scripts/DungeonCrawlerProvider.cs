using System;
using _Project.Scripts.Common.DependencyInjection;
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
        
        // Initialize all transverse services
        [Provide]
        public SceneLoader GetSceneLoader()
        {
            if (_sceneLoader == null)
                throw new Exception("SceneLoader not present in the scene");

            return _sceneLoader;
        }
    }
}
