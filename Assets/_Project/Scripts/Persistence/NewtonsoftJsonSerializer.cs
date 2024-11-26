using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using _Project.Scripts.Persistence;
using DungeonCrawler._Project.Scripts.Common;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Persistence
{
    /// <summary>
    /// Serializer permettant de serialiser les Scriptable Objects
    /// </summary>
    public class NewtonsoftJsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public NewtonsoftJsonSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new JsonConverter[] {new ScriptableObjectJsonConverter()}
            };
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}