using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace DungeonCrawler._Project.Scripts.SceneManagement
{
    public class SceneGroupManager
    {
        public event Action<Scene> OnSceneLoaded = s => { }; 
        public event Action<Scene> OnSceneUnloaded = s => {};
        public event Action OnSceneGroupLoaded = () => {};

        SceneGroup ActiveSceneGroup { get; set; }

        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
        {
            ActiveSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnloadScenes();
            int sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++) {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;

            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (var i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = group.Scenes[i];
                if (reloadDupScenes == false && loadedScenes.Contains(sceneData.Name)) 
                    continue;

                var operation = SceneManager.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
                operationGroup.Operations.Add(operation);
                
                operation.completed += _ => 
                {
                        Scene loadedScene = SceneManager.GetSceneByName(sceneData.Name);
                        if (loadedScene.IsValid())
                            OnSceneLoaded.Invoke(loadedScene);
                };
            }

            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }

            Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            if (activeScene.IsValid())
                SceneManager.SetActiveScene(activeScene);
        
            OnSceneGroupLoaded.Invoke();
        }

        public async Task UnloadScenes()
        {
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;

            int sceneCount = SceneManager.sceneCount;
            for (var i = sceneCount - 1; i > 0; i--)
            {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded) continue;

                var sceneName = sceneAt.name;
                // On conserve toujours la scene de démarrage active
                if (sceneName.Equals(activeScene) || sceneName == "Boostrapper") continue;
            
                scenes.Add(sceneName);
            }

            var operationGroup = new AsyncOperationGroup(scenes.Count);

            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if(operation == null) continue;
            
                operationGroup.Operations.Add(operation);
                
                Scene loadedScene = SceneManager.GetSceneByName(scene);
                OnSceneUnloaded.Invoke(loadedScene);
            }

            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }

            await Resources.UnloadUnusedAssets();
        } 
    } 

    public readonly struct AsyncOperationGroup {
        public readonly List<AsyncOperation> Operations;

        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(op => op.progress);
        public bool IsDone => Operations.All(op => op.isDone);

        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }
}