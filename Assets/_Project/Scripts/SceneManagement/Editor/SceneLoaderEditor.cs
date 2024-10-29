using UnityEditor;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.SceneManagement.Editor {
    [CustomEditor(typeof(SceneLoader))]
    public class SceneLoaderEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            SceneLoader sceneLoader = (SceneLoader) target;

            if (EditorApplication.isPlaying)
            {
                for (var i = 0; i < sceneLoader.SceneGroups.Length;i++)
                {
                    if(GUILayout.Button($"Load {sceneLoader.SceneGroups[i].GroupName} Scene Group")) 
                        LoadSceneGroup(sceneLoader, i);
                }
            }
        }

        static async void LoadSceneGroup(SceneLoader sceneLoader, int index) {
            await sceneLoader.LoadSceneGroup(index);
        }
    }
}