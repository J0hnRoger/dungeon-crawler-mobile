using _Project.Scripts.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class SceneLoadedEvent : IEvent
    {
        public Scene LoadedScene { get; }

        public SceneLoadedEvent(Scene loadedScene)
        {
            Debug.Log($"SceneLoadedEvent trigger: {loadedScene.name}");
            LoadedScene = loadedScene;
        }
    }
}
