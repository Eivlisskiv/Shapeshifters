using IgnitedBox.EventSystem;
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
        public struct ObjectiveTracking
        {
            public static implicit operator ObjectiveTracking(Transform t)
            {
                return new ObjectiveTracking()
                {
                    mobileTarget = t
                };
            }

            public static implicit operator ObjectiveTracking(Vector3 t)
            {
                return new ObjectiveTracking()
                {
                    staticTarget = t
                };
            }

            public static implicit operator ObjectiveTracking(Vector2 t)
            {
                return new ObjectiveTracking()
                {
                    staticTarget = t
                };
            }

            public static implicit operator Vector3(ObjectiveTracking ot)
                => ot.mobileTarget ? ot.mobileTarget.position : ot.staticTarget;

            Transform mobileTarget;
            Vector3 staticTarget;
        }

        public readonly GameObject element;
        public readonly RectTransform rect;

        public EventsHandler<ObjectiveHandler.ObjectiveEvents> Events
            = new EventsHandler<ObjectiveHandler.ObjectiveEvents>();

        public ObjectiveTracking? Track
        {
            get => _track;
            set
            {
                _track = value;
                Events.Invoke(ObjectiveHandler.ObjectiveEvents.TrackModified, 
                    this, _track);
            }
        }

        private ObjectiveTracking? _track;

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
            T t;
            if (elements.TryGetValue(name, out GameObject element))
                t = element.GetComponent<T>() ?? element.AddComponent<T>();
            else
            {
                element = new GameObject(name);
                Add(element);

                t = element.AddComponent<T>();
            }

            InitializeDefaults(t);
            action?.Invoke(t);

            return t;
        }

        public void Add(GameObject obj)
        {
            obj.transform.SetParent(container);

            obj.transform.localScale = new Vector3(1, 1, 1);

            elements.Add(obj.name, obj);
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
