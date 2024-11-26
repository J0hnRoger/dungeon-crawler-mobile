using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class SerializableSOAttribute : System.Attribute
    {
        public virtual object ToSerializedData(ScriptableObject so)
        {
            return JsonUtility.ToJson(so);
        }

        public virtual void FromSerializedData(ScriptableObject so, object data)
        {
            JsonUtility.FromJsonOverwrite((string)data, so);
        }
    }
}