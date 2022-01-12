using IgnitedBox.EventSystem;
using Scripts.OOP.Game_Modes.CustomLevels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives
{
    public class ObjectiveHandler : MonoBehaviour
    {
        public static ObjectiveHandler Instance { get; private set; }

        public GameObject elementPrefab;

        public ObjectiveElement Current =>
            elements.Count == 0 ? null : elements[elements.Count - 1];

        public enum EventTypes
        {
            Created, Removed, TrackModified
        }

        //[System.NonSerialized]
        public EventsHandler<EventTypes> Events { get; private set; }
        = new EventsHandler<EventTypes>();

        private readonly List<ObjectiveElement> elements
            = new List<ObjectiveElement>();

        private float mid;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            var prect = transform.parent.parent.GetComponent<RectTransform>();
            mid = -(prect.sizeDelta.y + 70) / 2;

            var rect = transform.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);

            rect.anchoredPosition = new Vector2(0.5f, 1);

            rect.localScale = new Vector3(1, 1, 1);

            rect.sizeDelta = new Vector3(0, 70, 0);

        }

        public ObjectiveElement CreateObjective(string name, 
            Color color = default, ObjectiveData data = null,
            System.Action<ObjectiveElement> func = null,
            System.Type objType = null)
        {
            GameObject element = Instantiate(elementPrefab);
            element.name = name;

            var image = element.GetComponent<Image>();
            if (image) image.color = color;

            ObjectiveElement oe = 
                (objType != null && objType.IsSubclassOf(typeof(ObjectiveElement))) ?
                (ObjectiveElement)System.Activator.CreateInstance(objType, element, data)
                : new ObjectiveElement(element);

            func?.Invoke(oe);

            Add(oe);

            Events.Invoke(EventTypes.Created, this, oe);

            return oe;
        }

        public T CreateObjective<T>(string name,
            Color color = default, ObjectiveData data = null,
            System.Action<ObjectiveElement> func = null)
            where T : ObjectiveElement
        {
            GameObject element = Instantiate(elementPrefab);
            element.name = name;

            var image = element.GetComponent<Image>();
            if (image) image.color = color;

            T oe = (T)System.Activator.CreateInstance(typeof(T), element, data);

            func?.Invoke(oe);

            Add(oe);

            Events.Invoke(EventTypes.Created, this, (ObjectiveElement)oe);

            return oe;
        }

        private void Add(ObjectiveElement element)
        {
            Current?.Shrink();

            element.element.transform.SetParent(transform);
            elements.Add(element);
            element.Spawn(mid);
        }

        public void Remove(ObjectiveElement element)
        {
            elements.Remove(element);
            Events.Invoke(EventTypes.Removed, this, element);
            element.Fade(this);

            Current?.Expand();
        }
    }
}
