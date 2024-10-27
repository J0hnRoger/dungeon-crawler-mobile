using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;

namespace DungeonCrawler._Project.Scripts.SceneManagement
{
    [Serializable]
    public class SceneGroup {
        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes = new();

        public string FindSceneNameByType(SceneType type) {
            return Scenes.FirstOrDefault(scene => scene.SceneType == type)?.Reference.Name;
        }
    }

    [Serializable]
    public class SceneData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
        public SceneType SceneType;
    }
}