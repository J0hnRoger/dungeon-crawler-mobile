using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace DungeonCrawler._Project.Scripts.Common.ScriptableObjects
{
    public class InlineInspectorAttribute : PropertyAttribute
    {
    }

    [CustomPropertyDrawer(typeof(InlineInspectorAttribute), true)]
    public class ScriptableObjectInspector : PropertyDrawer
    {
        // Hauteur d'une ligne dans l'inspecteur
        private const float LineHeight = 20f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var targetObject = property.objectReferenceValue as ScriptableObject;

            // Afficher le champ standard de l'objet
            position.height = LineHeight;
            property.objectReferenceValue =
                EditorGUI.ObjectField(position, label, targetObject, fieldInfo.FieldType, false);

            // Si un ScriptableObject est assigné, afficher ses propriétés
            if (targetObject != null)
            {
                EditorGUI.indentLevel++;

                SerializedObject serializedObject = new SerializedObject(targetObject);
                SerializedProperty prop = serializedObject.GetIterator();

                // Sauter la propriété "m_Script"
                prop.NextVisible(true);

                // Dessiner toutes les autres propriétés
                while (prop.NextVisible(false))
                {
                    position.y += LineHeight;
                    EditorGUI.PropertyField(position, prop, true);
                }

                if (serializedObject.hasModifiedProperties)
                    serializedObject.ApplyModifiedProperties();

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = LineHeight;

            var targetObject = property.objectReferenceValue as ScriptableObject;
            if (targetObject != null)
            {
                SerializedObject serializedObject = new SerializedObject(targetObject);
                SerializedProperty prop = serializedObject.GetIterator();

                prop.NextVisible(true); // Sauter m_Script

                while (prop.NextVisible(false))
                {
                    totalHeight += EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return totalHeight;
        }
    }
}
#endif