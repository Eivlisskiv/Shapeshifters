using System;
using UnityEngine;

namespace IgnitedBox.UnityUtilities
{
    public static class Components
    {
        public static T CreateGameObject<T>(string name = null, 
            Transform parent = null, Action<T> action = null)
            where T : Component
        {
            GameObject obj = new GameObject(name);
            return AddComponent(obj, parent, action);
        }

        public static T CreateGameObject<T>(
            out GameObject gameObject, string name = null,
            Transform parent = null, Action<T> action = null)
            where T : Component
        {
            gameObject = new GameObject(name);
            return AddComponent(gameObject, parent, action);
        }

        public static T AddComponent<T>(this GameObject obj, Action<T> action)
             where T : Component
        {
            T component = obj.AddComponent<T>();
            action(component);
            return component;
        }

        public static T AddComponent<T>(this GameObject obj, 
            Transform parent, Action<T> action)
            where T : Component
        {
            T component = obj.AddComponent<T>();
            if(parent)
            component.transform.SetParent(parent);
            action?.Invoke(component);
            return component;
        }
    }
}
