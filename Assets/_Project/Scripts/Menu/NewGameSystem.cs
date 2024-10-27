using System;
using System.Threading.Tasks;
using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Menu
{
    public class NewGameSystem : MonoBehaviour
    {
        [Inject] 
        private SceneLoader _sceneLoader;

        public async void HandleNewGame()
        {
            try
            {
                await _sceneLoader.LoadSceneGroup(1);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Loading New Game: {ex.Data}"); 
            }
        }
    }
}