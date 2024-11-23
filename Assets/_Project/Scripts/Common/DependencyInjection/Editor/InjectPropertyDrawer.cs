using _Project.Scripts.Common.DependencyInjection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.DependencyInjection.Editor
{
    [CustomPropertyDrawer(typeof(InjectAttribute))]
    public class InjectPropertyDrawer : PropertyDrawer
    {
        private Texture2D icon;

        Texture2D LoadIcon()
        {
            if (icon == null)
            {
                icon = AssetDatabase.LoadAssetAtPath<Texture2D>(
                    "Assets/_Project/Scripts/Common/DependencyInjection/Editor/icon.png");
            }

            return icon;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            icon = LoadIcon();
            var iconRect = new Rect(position.x, position.y, 20, 20);
            position.xMin += 24;

            if (icon != null)
            {
                var savedColor = GUI.color;
                // Vérifie si la propriété est une référence d'objet avant d'appliquer `objectReferenceValue`.
                if (property.propertyPath.EndsWith(".Array.data[0]"))
                {
                    // Remonter à la propriété parente (la List elle-même)
                    string listPath = property.propertyPath.Replace(".Array.data[0]", "");
                    var listProperty = property.serializedObject.FindProperty(listPath);
                    GUI.color = listProperty.arraySize > 0 ? Color.green : savedColor;
                }
                else if (property.propertyType == SerializedPropertyType.Generic)
                {
                    var valueProperty = property.FindPropertyRelative("_value");
                    if (valueProperty != null)
                    {
                        GUI.color = valueProperty.objectReferenceValue != null ? Color.green : savedColor;
                    }
                }
                else if (property.propertyType == SerializedPropertyType.ObjectReference)
                {
                    GUI.color = property.objectReferenceValue == null ? savedColor : Color.green;
                }

                GUI.DrawTexture(iconRect, icon);
                GUI.color = savedColor;
            }

            // Handle standard property
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Generic && property.isArray)
            {
                float totalHeight = EditorGUIUtility.singleLineHeight; // To allow for a foldout
                if (property.isExpanded) // Check to expand the elements
                {
                    for (int i = 0; i < property.arraySize; i++)
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i), true);
                    }
                }

                return totalHeight;
            }

            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}