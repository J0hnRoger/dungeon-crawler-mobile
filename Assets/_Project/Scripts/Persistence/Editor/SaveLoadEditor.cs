using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Persistence.Editor
{
    [CustomEditor(typeof(SaveLoadSystem))]
    public class SaveLoadEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaveLoadSystem saveLoadSystem = (SaveLoadSystem)target;
            string gameName = saveLoadSystem._gameData.Name;

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