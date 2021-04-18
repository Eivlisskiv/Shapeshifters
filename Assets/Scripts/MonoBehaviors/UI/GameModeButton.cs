using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : MonoBehaviour
{
    private RectTransform desc;
    private Text text;
    private Vector3 size;

    private bool mouseIn;

    void Init()
    {
        if (!desc)
        {
            var panel = transform.parent.GetChild(1);
            desc = panel.GetComponent<RectTransform>();

            var t = panel.GetChild(0);
            var t2 = t.GetComponent<RectTransform>();
            text = t.GetComponent<Text>();
            size = t2.sizeDelta;
        }
    }

    public void MouseEnter()
    {
        mouseIn = true;
        Init();

        desc.Tween<RectTransform, Vector3, RectSizeTween>
            (size + new Vector3(5, 5, 0), 0.2f,
            easing: BackEasing.Out, 
            callback: () => 
            {
                if (mouseIn) text.color = Color.white;
            });
    }

    public void MouseExit()
    {
        mouseIn = false;
        Init();
        text.color = new Color(0, 0, 0, 0);
        desc.Tween<RectTransform, Vector3, RectSizeTween>(
            new Vector3(0, 0, 0), 0.2f);
    }
}
