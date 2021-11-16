using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using Scripts.MonoBehaviors.UI.Menu;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    public bool Enabled 
    {
        get => enabled;
        set 
        {
            enabled = value;
            TweenColor(value ? Color.white : Color.red, 1);
        }
    }

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
        if (!Enabled) return;
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
        if (!Enabled) return;

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
            new Vector3(1.15f, 1.15f, 1), .8f, easing: ElasticEasing.Out)
            .scaledTime = false;

        TweenColor(Color.cyan, 0.5f);
    }

    protected virtual void OnDeselect()
    {
        transform.parent.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(1, 1, 1), .3f, easing: BackEasing.Out)
            .scaledTime = false;

        TweenColor(Color.white, .2f);
    }

    protected virtual void TweenColor(Color target, float time)
    {
        var image = transform.parent.GetComponent<Image>();
        if (image)
        {
            image.Tween<Graphic, Color, GraphicColorTween>
                (target, time).scaledTime = false;
        }
    }
}
