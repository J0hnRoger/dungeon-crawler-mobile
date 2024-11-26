using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Persistence
{
    public class ScriptableObjectJsonConverter : JsonConverter<ScriptableObject>
    {
        private class SOReference
        {
            public string Guid { get; set; }
            public string AssetPath { get; set; }
            public string TypeName { get; set; }
        }

        public override ScriptableObject ReadJson(JsonReader reader, 
            Type objectType, 
            ScriptableObject existingValue, 
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            // Désérialiser la référence
            var reference = serializer.Deserialize<SOReference>(reader);
            if (reference == null) return null;

            // Charger le SO depuis son chemin
            return AssetDatabase.LoadAssetAtPath<ScriptableObject>(reference.AssetPath);
        }

        public override void WriteJson(JsonWriter writer, 
            ScriptableObject value, 
            JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var reference = new SOReference
            {
                Guid = GetSOGuid(value),
                AssetPath = AssetDatabase.GetAssetPath(value),
                TypeName = value.GetType().FullName
            };

            serializer.Serialize(writer, reference);
        }

        private string GetSOGuid(ScriptableObject so)
        {
            var guidField = so.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.GetCustomAttribute<GuidAttribute>() != null);

            return guidField != null 
                ? (string)guidField.GetValue(so) 
                : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(so));
        }
    }
}
