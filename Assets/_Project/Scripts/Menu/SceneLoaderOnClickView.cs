using System;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Menu
{
    public class SceneLoaderOnClickView : MonoBehaviour
    {
        [Inject] 
        private SceneLoader _sceneLoader;
        
        [SerializeField] private int _sceneIndex;

        public async void LoadScene()
        {
            try
            {
                await _sceneLoader.LoadSceneGroup(_sceneIndex);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Loading New Game: {ex.Data}"); 
            }
        }
    }
}