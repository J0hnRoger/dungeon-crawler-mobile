using DungeonCrawler._Project.Scripts.Persistence;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
namespace _Project.Scripts.Persistence.Editor
{
    [CustomEditor(typeof(SaveLoadSystem))]
    public class SaveLoadEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaveLoadSystem saveLoadSystem = (SaveLoadSystem)target;
            string gameName = saveLoadSystem.GameData.Name;

            DrawDefaultInspector();

            if (GUILayout.Button("Save Game"))
                saveLoadSystem.SaveGame();

            if (GUILayout.Button("Load Game"))
                saveLoadSystem.LoadGame(gameName);

            if (GUILayout.Button("Delete Game"))
                saveLoadSystem.DeleteGame(gameName);
        }
    }
}
#endif