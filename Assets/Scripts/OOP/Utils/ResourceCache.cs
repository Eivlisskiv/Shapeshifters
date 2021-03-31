using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Utils
{
    public class ResourceCache<T> where T : Object
    {
        string path;

        Dictionary<string, T> prefabs;

        public ResourceCache(string path)
        {
            this.path = path;
            prefabs = new Dictionary<string, T>();
        }

        public T Instantiate(string name)
        {
            if (!prefabs.TryGetValue(name, out T prefab))
            {
                prefab = Resources.Load<T>(path + name);
                prefabs.Add(name, prefab);
            }

            return (T)Object.Instantiate(prefab);
        }

        public void Clear() => prefabs.Clear();
    }
}
