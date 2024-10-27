using DungeonCrawler._Project.Scripts.Common;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts
{
    public class Boostrapper : PersistentSingleton<Boostrapper>
    {
        static readonly int sceneIndex = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            Debug.Log("Bootstrapper...");
#if UNITY_EDITOR
            // Set the bootstrapper scene to be the play mode start scene when running in the editor
            // This will cause the bootstrapper scene to be loaded first (and only once) when entering
            // play mode from the Unity Editor, regardless of which scene is currently active.
            EditorSceneManager.playModeStartScene =
                AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[sceneIndex].path);
#endif
        }
    }
}