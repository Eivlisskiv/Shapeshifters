using UnityEngine;

namespace Scripts.OOP.Perks
{
    public abstract class EmitterPerk : Perk
    {
        private GameObject prefab;

        protected abstract string RessourcePath { get; }

        protected override void Start()
        {
            base.Start();
            prefab = Resources.Load<GameObject>(RessourcePath);
        }

        protected GameObject SpawnPrefab(Vector3 position, Vector3? rotation = null, 
            Transform parent = null)
        {
            if (!prefab)
            {
                Debug.Log($"{RessourcePath} failed to load prefab");
                return null;
            }

            GameObject go = Object.Instantiate(prefab);
            if(parent) go.transform.SetParent(parent);
            go.transform.position = position;
            if (rotation.HasValue) 
                go.transform.rotation = Quaternion.Euler(rotation.Value);
            return go;
        }
    }
}
