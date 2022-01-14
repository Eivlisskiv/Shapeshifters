using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives
{
    public class ObjectiveElement
    {
        private static readonly Font font
             = Resources.Load<Font>("Fonts/ThaleahFat_TTF");

        private class ObjectiveContent
        {
            public readonly GameObject gameObject;

            public readonly RectTransform rect;

            public float width;

            public ObjectiveContent(GameObject go, float width = 1)
            {
                gameObject = go;
                rect = go.GetComponent<RectTransform>();
                this.width = width;
            }
        }

        public readonly GameObject element;
        public readonly RectTransform rect;
        public readonly RectTransform InnerRect;

        private float widthWeights;

        public ObjectiveTracking? Track
        {
            get => _track;
            set
            {
                _track = value;
                ObjectiveHandler.Instance.Events.Invoke
                    (ObjectiveHandler.EventTypes.TrackModified, 
                    this, _track);
            }
        }

        private ObjectiveTracking? _track;

        private readonly Dictionary<string, ObjectiveContent> elements =
            new Dictionary<string, ObjectiveContent>();

        private readonly Transform container;
        protected bool IsSpawning { get; private set; }

        protected bool constructing;

        public ObjectiveElement(GameObject element)
        {
            constructing = true;

            this.element = element;
            container = element.transform.GetChild(0);
            rect = element.GetComponent<RectTransform>();
            var child = rect.GetChild(0);
            InnerRect = child.GetComponent<RectTransform>();
        }

        protected virtual void OnReady()
        {
            IsSpawning = false;
        }

        protected virtual void OnRemoved(ObjectiveHandler handler) 
        {
            handler.Events.CleanInstace(this);
        }

        public T Get<T>(string name, Action<T> action = null, float width = 1) where T : Graphic
        {
            T t;
            if (elements.TryGetValue(name, out ObjectiveContent content))
            { 
                t = content.gameObject.GetComponent<T>() ?? content.gameObject.AddComponent<T>();
                InitializeDefaults(t);
                action?.Invoke(t);
                return t;
            }

            GameObject gameObject = new GameObject(name);
            t = gameObject.AddComponent<T>();

            InitializeDefaults(t);
            action?.Invoke(t);
            Add(gameObject, width);
            return t;
        }

        public void AddContained(GameObject obj, float width = 1)
        {
            GameObject container = new GameObject($"{obj.name}_Container");
            obj.transform.SetParent(container.transform, true);
            Add(container, width);
        }

        public void Add(GameObject obj, float width = 1)
        {
            var scale = obj.transform.localScale;
            obj.transform.SetParent(container);
            obj.transform.localScale = scale;

            elements.Add(obj.name, new ObjectiveContent(obj, width));
            widthWeights += width;
            if (!constructing) UpdateWidths();
        }

        public void ChangeWidth(string key, float width)
        {
            if (!elements.TryGetValue(key, out ObjectiveContent content)) return;
            if (content.width == width) return;
            widthWeights += (width - content.width);
            content.width = width;
            if(!constructing) UpdateWidths();
        }

        private void UpdateWidths()
        {
            foreach (KeyValuePair<string, ObjectiveContent> keypair in elements)
                UpdateWidth(keypair.Value);
        }

        private void UpdateWidth(ObjectiveContent content)
        {
            float tsize = /*constructing || IsSpawning ? 650 :*/ InnerRect.rect.width;
            float width = tsize * content.width / widthWeights;
            content.rect.sizeDelta = new Vector2(width, content.rect.sizeDelta.y);
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
            IsSpawning = true;
            constructing = false;
            UpdateWidths();

            rect.localScale = new Vector2(0, 0);
            rect.localPosition = new Vector3(0, y, 0);

            rect.Tween<Transform, Vector3, ScaleTween>
            (
                new Vector3(1, 1, 1), 1, easing: ExponentEasing.Out,
                callback: SpawnStep2
            );
        }

        private void SpawnStep2()
        {
            rect.Tween<Transform, Vector3, PositionTween>
            (
                new Vector3(0, -rect.sizeDelta.y / 2, 0),
                0.3f, easing: ExponentEasing.Out, callback: OnReady
            );
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
            if (IsSpawning) return;

            rect.Tween<Transform, Vector3, ScaleTween>
                (new Vector3(1, 1, 1), 1, 1.5f, ExponentEasing.Out);

            rect.Tween<Transform, Vector3, PositionTween>
                (new Vector3(0, -rect.sizeDelta.y / 2, 0),
                1, 1.5f, ExponentEasing.Out);
        }

        public void Fade(ObjectiveHandler handler)
        {
            OnRemoved(handler);
            rect.Tween<Transform, Vector3, ScaleTween>
                (new Vector3(0, 0, 0), 1, easing: BackEasing.In,
                callback: () => UnityEngine.Object.Destroy(element));
        }

        public void Bounce()
        {
            rect.Tween<Transform, Vector3, PositionTween>
                (rect.localPosition + new Vector3(0, -30, 0),
                0.5f, 0, d => (-4*Math.Pow(d - 0.5, 2)) + 1);
        }
    }
}
