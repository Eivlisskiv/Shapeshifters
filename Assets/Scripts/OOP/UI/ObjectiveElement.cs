using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.UI
{
    public class ObjectiveElement
    {
        public readonly GameObject element;
        public readonly RectTransform rect;
        private readonly Dictionary<string, GameObject> elements = new Dictionary<string, GameObject>();

        private readonly Transform container;

        private readonly Font font;

        public ObjectiveElement(GameObject element)
        {
            this.element = element;
            container = element.transform.GetChild(0);
            rect = element.GetComponent<RectTransform>();

            font = Resources.Load<Font>("Fonts/ThaleahFat_TTF");
        }

        public T Get<T>(string name, Action<T> action = null) where T : Graphic
        {
            if (elements.TryGetValue(name, out GameObject element))
                return element.GetComponent<T>() ?? element.AddComponent<T>();

            element = new GameObject(name, typeof(T));
            element.transform.SetParent(container);

            element.transform.localScale = new Vector3(1, 1, 1);

            elements.Add(name, element);

            var t = element.GetComponent<T>();
            InitializeDefaults(t);
            action?.Invoke(t);

            return t;
        }

        private void InitializeDefaults(Graphic graph)
        {
            if (graph is Text text) SetText(text);
        }

        private void SetText(Text text)
        {
            text.fontSize = 36;

            text.resizeTextForBestFit = true;
            text.resizeTextMaxSize = 36;

            text.alignment = TextAnchor.MiddleLeft;

            if (font) text.font = font;
        }

        public void Spawn(float y)
        {
            rect.localScale = new Vector2(0, 0);
            rect.localPosition = new Vector3(0, y, 0);

            rect.Tween<Transform, Vector3, ScaleTween>
                (new Vector3(1, 1, 1), 1, easing: ExponentEasing.Out,
                callback: () => rect.Tween<Transform, Vector3, PositionTween>
                (new Vector3(0, -rect.sizeDelta.y / 2, 0),
                0.2f, easing: ExponentEasing.Out));
        }

        public void Shrink()
        {
            rect.Tween<Transform, Vector3, ScaleTween>
                (new Vector3(1, 0, 1), 0.5f, easing: ExponentEasing.Out);

            rect.Tween<Transform, Vector3, PositionTween>
                (rect.localPosition + new Vector3(0, 100, 0),
                1f, easing: BackEasing.In);
        }

        public void Expand()
        {
            rect.Tween<Transform, Vector3, ScaleTween>
                (new Vector3(1, 1, 1), 1, 1.5f, ExponentEasing.Out);

            rect.Tween<Transform, Vector3, PositionTween>
                (new Vector3(0, -rect.sizeDelta.y / 2, 0),
                1, 1.5f, ExponentEasing.Out);
        }

        public void Fade()
        {
            rect.Tween<Transform, Vector3, ScaleTween>
                (new Vector3(0, 0, 0), 1, easing: BackEasing.In,
                callback: () => UnityEngine.Object.Destroy(element));
        }
    }
}
