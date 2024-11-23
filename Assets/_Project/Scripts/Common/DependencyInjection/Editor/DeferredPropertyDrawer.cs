using UnityEditor;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection.Editor
{
    [CustomPropertyDrawer(typeof(Deferred<>))]
    public class DeferredPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}