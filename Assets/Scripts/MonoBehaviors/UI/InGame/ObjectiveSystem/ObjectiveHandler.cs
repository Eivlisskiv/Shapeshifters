using IgnitedBox.EventSystem;
using Scripts.OOP.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHandler : MonoBehaviour
{
    public GameObject elementPrefab;

    public ObjectiveElement Current => 
        elements.Count == 0 ? null : elements[elements.Count - 1];

    public enum ObjectiveEvents
    {
        Created, TrackModified
    }

    [System.NonSerialized]
    public EventsHandler<ObjectiveEvents> Events
    = new EventsHandler<ObjectiveEvents>();

    private readonly List<ObjectiveElement> elements 
        = new List<ObjectiveElement>();

    private float mid;

    private void Start()
    {
        var prect = transform.parent.GetComponent<RectTransform>();
        mid = -(prect.sizeDelta.y + 70) / 2;

        var rect = transform.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);

        rect.anchoredPosition = new Vector2(0.5f, 1);

        rect.localScale = new Vector3(1, 1, 1);

        rect.sizeDelta = new Vector3(0, 70, 0);

    }

    public ObjectiveElement CreateObjective(string name, Color color = default, 
        System.Action<ObjectiveElement> func = null)
    {
        GameObject element = Instantiate(elementPrefab);
        element.name = name;

        var image = element.GetComponent<Image>();
        if(image) image.color = color;

        ObjectiveElement oe = new ObjectiveElement(element);

        func?.Invoke(oe);

        Add(oe);

        Events.Invoke(ObjectiveEvents.Created, this, oe);

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
        element.Fade();

        Current?.Expand();
    }
}
