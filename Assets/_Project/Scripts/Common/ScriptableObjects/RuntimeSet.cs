using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Common.ScriptableObjects
{
    public abstract class RuntimeSetSO<T> : ScriptableObject
    {
        public List<T> Items = new List<T>();

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }
    }
}