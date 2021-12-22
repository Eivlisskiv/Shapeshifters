using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : GeneralButton
{
    private RectTransform desc;
    private Text text;

    protected override void OnStart()
    {
        var panel = transform.parent.GetChild(1);
        desc = panel.GetComponent<RectTransform>();

        var t = panel.GetChild(0);
        var t2 = t.GetComponent<RectTransform>();
        text = t.GetComponent<Text>();
        desc.sizeDelta = t2.sizeDelta + new Vector2(5, 5);
        desc.localScale = new Vector3(0, 1, 1);
    }

    protected override void OnSelect()
    {
        base.OnSelect();
        desc.Tween<Transform, Vector3, ScaleTween>
            (new Vector3(1, 1, 1), 0.2f,
            easing: BackEasing.Out,
            callback: () =>
            {
                if (Selected) text.color = Color.white;
                if(desc.sizeDelta.y == 5)
                {
                    var t2 = text.GetComponent<RectTransform>();
                    desc.sizeDelta = t2.sizeDelta + new Vector2(5, 5);
                }
            }
        );
    }

    protected override void OnDeselect()
    {
        base.OnDeselect();
        text.color = new Color(0, 0, 0, 0);
        desc.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(0, 1, 1), 0.2f);
    }
}
