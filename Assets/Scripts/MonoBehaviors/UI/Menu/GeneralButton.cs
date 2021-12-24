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
    public enum GroupType
    {
        Focus, Selected
    }

    public static void GroupOffStatus(string group)
    {
        GroupOffStatus(GroupType.Focus, group);
        GroupOffStatus(GroupType.Selected, group);
    }

    public static void GroupOffStatus(GroupType gt, string group)
    {
        Dictionary<string, GeneralButton> groups = GetGroup(gt);

        if (!groups.TryGetValue(group, out GeneralButton selected)) return;
        if (!selected) return;
        switch (gt)
        {
            case GroupType.Focus: selected.ChangeFocus(false); break;
            case GroupType.Selected: selected.ChangeSelect(false); break;
        }
        groups[group] = null;
    }

    private static Dictionary<string, GeneralButton> GetGroup(GroupType gt)
    {
        switch (gt)
        {
            case GroupType.Focus: return GroupFocused;
            case GroupType.Selected: return GroupSelected;
        };

        return null;
    }

    private static readonly Dictionary<string, GeneralButton> GroupFocused = new Dictionary<string, GeneralButton>();
    private static readonly Dictionary<string, GeneralButton> GroupSelected = new Dictionary<string, GeneralButton>();

    public bool HasGroup => !string.IsNullOrEmpty(group);
    
    [SerializeField]
    public string group;

    public bool selectBeforePress = true;

    public Color Color => HasGroup && Selected ? selectColor : offColor;

    public Color offColor = Color.white;
    public Color focusColor = Color.cyan;
    public Color selectColor = Color.yellow;

    public bool Enabled 
    {
        get => enabled;
        set 
        {
            enabled = value;
            ChangeFocus(false);
            ChangeSelect(false);
            TweenColor(value ? Color : Color.red, 1);
        }
    }

    public Button Button { get; private set;}
    protected bool Focused { get; private set; }
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

    public void ChangeFocus(bool focus)
    {
        if (!Enabled) return;
        if (focus == Focused) return;

        Focused = focus;
        if (focus)
        {
            SelectGroup(GroupType.Focus);
            OnFocus();
        }
        else
        {
            DeselectGroup(GroupType.Focus);
            OnUnfocus();
        }
    }

    public void ChangeSelect(bool selected)
    {
        if (!Enabled) return;
        if (selected == Selected) return;

        Selected = selected;
        if (Selected) SelectGroup(GroupType.Selected);
        else  DeselectGroup(GroupType.Selected);

        if (!Focused) TweenColor(Color, .2f);
    }

    private void SelectGroup(GroupType gt)
    {
        if (!HasGroup) return;

        var groups = GetGroup(gt);

        if(!groups.TryGetValue(group, out GeneralButton current))
        {
            groups.Add(group, this);
            return;
        }

        if (current)
        {
            switch (gt)
            {
                case GroupType.Focus: current.ChangeFocus(false); break;
                case GroupType.Selected: current.ChangeSelect(false); break;
            }
            
        }

        groups[group] = this;
    }

    private void DeselectGroup(GroupType gt)
    {
        if (!HasGroup) return;

        var groups = GetGroup(gt);

        if (!groups.ContainsKey(group)) return;

        groups[group] = null;
    }

    public void ButtonPress()
    {
        if (!Enabled) return;

        if (!Focused)
        {
            ChangeFocus(true);

            if(selectBeforePress) return;
        }

        //Group buttons are selected, not re clicked
        if (Selected && HasGroup) return;

        ChangeSelect(!Selected);

        OnActivate();
        OnPress.Invoke();
    }

    protected virtual void OnActivate() { }

    protected virtual void OnFocus()
    {
        transform.parent.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(1.15f, 1.15f, 1), .8f, easing: ElasticEasing.Out)
            .scaledTime = false;

        TweenColor(focusColor, 0.5f);
    }

    protected virtual void OnUnfocus()
    {
        transform.parent.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(1, 1, 1), .3f, easing: BackEasing.Out)
            .scaledTime = false;

        TweenColor(Color, .2f);
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
