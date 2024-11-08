#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _Project.Scripts.Common;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.EventBus
{
    public class EventDebuggerWindow : EditorWindow
    {
        [SerializeField] private int _selectedEventIndex;
        private Type[] _eventTypes;
        private string[] _eventNames;
        private Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        [MenuItem("Window/Event Debugger")]
        public static void ShowWindow()
        {
            var window = GetWindow<EventDebuggerWindow>("Event Debugger");
            window.minSize = new Vector2(400, 600);
        }

        private void OnEnable()
        {
            // Trouve tous les types qui implémentent IEvent
            _eventTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IEvent).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .OrderBy(type => type.Name)
                .ToArray();

            _eventNames = _eventTypes.Select(t => t.Name).ToArray();
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Only available in Play Mode", MessageType.Info);
                return;
            }

            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Event Debugger", EditorStyles.boldLabel);

                // Dropdown pour sélectionner l'événement
                int newIndex = EditorGUILayout.Popup("Select Event", _selectedEventIndex, _eventNames);
                if (newIndex != _selectedEventIndex)
                {
                    _selectedEventIndex = newIndex;
                }

                if (_selectedEventIndex >= 0 && _selectedEventIndex < _eventTypes.Length)
                {
                    Type selectedType = _eventTypes[_selectedEventIndex];

                    // Affiche les propriétés de l'événement si il en a
                    var properties = selectedType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    if (properties.Length > 0)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Event Properties:", EditorStyles.boldLabel);
                        DrawEventProperties(selectedType);
                    }

                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Trigger Event"))
                        {
                            TriggerEvent(selectedType);
                        }

                        if (GUILayout.Button("Reset Values", GUILayout.Width(100)))
                        {
                            ResetPropertyValues(selectedType);
                        }
                    }
                }
            }
        }

        private void DrawEventProperties(Type selectedType)
        {
            var properties = selectedType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                string key = $"{selectedType.Name}.{prop.Name}";
                if (!_propertyValues.ContainsKey(key))
                {
                    _propertyValues[key] = GetDefaultValue(prop.PropertyType);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(prop.Name, GUILayout.Width(150));

                    if (prop.PropertyType == typeof(bool))
                    {
                        _propertyValues[key] = EditorGUILayout.Toggle((bool)_propertyValues[key]);
                    }
                    // Custom Drawer for listing all Prefab in Assets/Level/Resources/Levels
                    else if (Attribute.IsDefined(prop, typeof(LevelPathAttribute)))
                    {
                        var levels = Resources.LoadAll<GameObject>("Levels")
                            .Select(go => "Levels/" + go.name)
                            .ToArray();

                        if (levels.Length == 0)
                        {
                            EditorGUILayout.HelpBox("No levels found in Resources/Levels", MessageType.Warning);
                            continue;
                        }

                        // S'assurer que la valeur initiale est valide
                        if (_propertyValues[key] == null || !levels.Contains(_propertyValues[key] as string))
                        {
                            _propertyValues[key] = levels[0];
                        }

                        // Trouver l'index actuel
                        int currentIndex = Array.IndexOf(levels, _propertyValues[key] as string);

                        // Afficher la dropdown et mettre à jour la valeur
                        EditorGUI.BeginChangeCheck();
                        int newIndex = EditorGUILayout.Popup("Levels dispo", currentIndex, levels);
                        if (EditorGUI.EndChangeCheck())
                        {
                            _propertyValues[key] = levels[newIndex];
                            GUI.changed = true;
                        }
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        _propertyValues[key] = EditorGUILayout.IntField((int)_propertyValues[key]);
                    }
                    else if (prop.PropertyType == typeof(float))
                    {
                        _propertyValues[key] = EditorGUILayout.FloatField((float)_propertyValues[key]);
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        _propertyValues[key] = EditorGUILayout.TextField((string)_propertyValues[key]);
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        _propertyValues[key] = EditorGUILayout.EnumPopup((Enum)_propertyValues[key]);
                    }
                    else if (prop.PropertyType == typeof(Vector2))
                    {
                        _propertyValues[key] = EditorGUILayout.Vector2Field("", (Vector2)_propertyValues[key]);
                    }
                    else if (prop.PropertyType == typeof(Vector3))
                    {
                        _propertyValues[key] = EditorGUILayout.Vector3Field("", (Vector3)_propertyValues[key]);
                    }
                    else
                    {
                        EditorGUILayout.LabelField($"[Type non supporté: {prop.PropertyType.Name}]");
                    }
                }
            }
        }

        private void DrawLevelPathField()
        {
        }

        private object GetDefaultValue(Type t)
        {
            if (t == typeof(bool)) return false;
            if (t == typeof(int)) return 0;
            if (t == typeof(float)) return 0f;
            if (t == typeof(string)) return "";
            if (t == typeof(Vector2)) return Vector2.zero;
            if (t == typeof(Vector3)) return Vector3.zero;
            if (t.IsEnum) return Enum.GetValues(t).GetValue(0);
            return null;
        }

        private void ResetPropertyValues(Type eventType)
        {
            var properties = eventType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                string key = $"{eventType.Name}.{prop.Name}";
                _propertyValues[key] = GetDefaultValue(prop.PropertyType);
            }
        }

        private void TriggerEvent(Type eventType)
        {
            try
            {
                // Crée une nouvelle instance de l'événement
                var evt = Activator.CreateInstance(eventType) as IEvent;

                // Applique les valeurs des propriétés
                foreach (var prop in eventType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    string key = $"{eventType.Name}.{prop.Name}";
                    if (_propertyValues.ContainsKey(key))
                    {
                        try
                        {
                            prop.SetValue(evt, _propertyValues[key]);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Error setting property {prop.Name}: {e.Message}");
                        }
                    }
                }

                // Déclenche l'événement
                var eventBusType = typeof(EventBus<>).MakeGenericType(eventType);
                var raiseMethod = eventBusType.GetMethod("Raise");
                raiseMethod?.Invoke(null, new object[] { evt });

                Debug.Log($"Event triggered: {eventType.Name}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error triggering event: {e.Message}");
            }
        }
    }
}
#endif