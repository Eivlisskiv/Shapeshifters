using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using Scripts.MonoBehaviors.UI.Menu;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneralButton : MonoBehaviour
{
    public static void DeselectGroup(string group)
    {
        if (!GroupSelected.TryGetValue(group, out GeneralButton selected)) return;
        if (!selected) return;
        selected.ChangeSelect(false);
        GroupSelected[group] = null;
    }

    private static readonly Dictionary<string, GeneralButton> GroupSelected = new Dictionary<string, GeneralButton>();

    [SerializeField]
    public string group;

    protected Button Button { get; private set;}
    protected bool Selected { get; private set; }

    public UnityEvent OnPress => onPress;

    [SerializeField]
    private UnityEvent onPress = new UnityEvent();

    private void Start()
    {
        Button = GetComponent<Button>();
        if (Button) Button.onClick.RemoveAllListeners();
        else Button = gameObject.AddComponent<Button>();
        Button.targetGraphic = GetComponent<Graphic>();
        Button.onClick.AddListener(ButtonPress);

        var triggers = gameObject.AddComponent<GeneralButtonTriggers>();
        triggers.Button = this;

        OnStart();
    }

    protected virtual void OnStart() { }

    public void ChangeSelect(bool selected)
    {
        if (!enabled) return;
        if (selected == Selected) return;

        Selected = selected;
        if (selected)
        {
            SelectGroup();
            OnSelect();
        }
        else
        {
            DeselectGroup();
            OnDeselect();
        }
    }

    private void SelectGroup()
    {
        if (group != null && group.Length == 0) return;
        
        if(!GroupSelected.TryGetValue(group, out GeneralButton current))
        {
            GroupSelected.Add(group, this);
            return;
        }

        if(current) current.ChangeSelect(false);
        GroupSelected[group] = this;
    }

    private void DeselectGroup()
    {
        if (group != null && group.Length == 0) return;

        if (!GroupSelected.TryGetValue(group, out _)) return;

        GroupSelected[group] = null;
    }

    public void ButtonPress()
    {
        if (!enabled) return;

        if (!Selected)
        {
            ChangeSelect(true);
            return;
        }

        OnActivate();
        OnPress.Invoke();
    }

    protected virtual void OnActivate() { }

    protected virtual void OnSelect()
    {
        transform.parent.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(1.15f, 1.15f, 1), .8f, easing: ElasticEasing.Out);
    }

    protected virtual void OnDeselect()
    {
        transform.parent.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(1, 1, 1), .3f, easing: BackEasing.Out);
    }
}
