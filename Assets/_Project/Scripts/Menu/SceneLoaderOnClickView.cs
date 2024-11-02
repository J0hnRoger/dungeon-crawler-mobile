using _Project.Scripts.Common.DependencyInjection;
using DungeonCrawler._Project.Scripts.Dungeon;
using DungeonCrawler._Project.Scripts.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Menu
{
    public class SceneLoaderOnClickView : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;

        [SerializeField] private DungeonCrawlerScenes _scene;

        public async void LoadScene()
        {
            if (_sceneLoader == null)
                Debug.LogWarning($"[SceneLoader] Load Scene: {_scene}");
            else
                await _sceneLoader.LoadSceneGroup((int)_scene);
        }
    }
}