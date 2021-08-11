using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Utils
{
    public class ResourceCache<T> where T : Object
    {
        readonly string path;
        readonly Dictionary<string, T> prefabs;

        public ResourceCache(string path)
        {
            this.path = path;
            prefabs = new Dictionary<string, T>();
        }

        public T Instantiate(string name)
        {
            if (TryGetPrefab(name, out T prefab))
                return Object.Instantiate(prefab);

            return null;
        }

        public bool TryGetPrefab(string name, out T prefab)
        {
            if (!prefabs.TryGetValue(name, out prefab))
            {
                prefab = Resources.Load<T>(path + name);
                if(prefab) prefabs.Add(name, prefab);
            }

            return prefab;
        }

        public void Clear() => prefabs.Clear();
    }
}
