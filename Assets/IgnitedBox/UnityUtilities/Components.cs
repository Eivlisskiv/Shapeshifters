﻿using System;
using UnityEngine;

namespace IgnitedBox.UnityUtilities
{
    public static class Components
    {
        public static void Center(this RectTransform rect)
        {
            Vector2 half = Vector2.one / 2;
            rect.anchorMin = half;
            rect.anchorMax = half;
            rect.pivot = half;
        }

        public static void CenterStretch(this RectTransform rect)
        {
            rect.sizeDelta = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
        }

        public static void DestroyChildren(this Transform t)
        {
            if (!t) return;
            int c = t.childCount;
            if (c < 1) return;
            for(int i = 0; i < c; i++)
            {
                var child = t.GetChild(i);
                if (child) UnityEngine.Object.Destroy(child.gameObject);
            }    
        }

        public static bool IsPrefab(this GameObject obj)
            => obj.scene.rootCount == 0 && string.IsNullOrEmpty(obj.scene.name);

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
            component.transform.SetParent(parent, false);
            action?.Invoke(component);
            return component;
        }
    }
}
