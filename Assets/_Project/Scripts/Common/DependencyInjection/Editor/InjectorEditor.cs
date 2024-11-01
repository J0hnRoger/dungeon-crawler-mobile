using _Project.Scripts.Common.DependencyInjection;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection.Editor
{
  [CustomEditor(typeof(Injector))]
    public class InjectorEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Injector injector = (Injector) target;

            if (GUILayout.Button("Validate Dependencies")) {
                injector.ValidateDependencies();
            }

            if (GUILayout.Button("Clear All Injectable Fields")) {
                injector.ClearDependencies();
                EditorUtility.SetDirty(injector);
            }
        }
    }
}