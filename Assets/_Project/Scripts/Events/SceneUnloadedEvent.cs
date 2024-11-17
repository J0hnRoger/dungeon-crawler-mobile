using _Project.Scripts.Common;
using UnityEngine.SceneManagement;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class SceneUnloadedEvent : IEvent
    {
        public Scene UnloadedScene { get; }

        public SceneUnloadedEvent(Scene unloadedScene)
        {
            UnloadedScene = unloadedScene;
        }
    }
}