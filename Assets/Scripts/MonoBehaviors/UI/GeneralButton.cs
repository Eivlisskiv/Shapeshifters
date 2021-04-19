using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using UnityEngine;

public class GeneralButton : MonoBehaviour
{
    public void MouseEnter()
    {
        transform.parent.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(1.15f, 1.15f, 1), .8f, easing: ElasticEasing.Out);
    }

    public void MouseExit()
    {
        transform.parent.Tween<Transform, Vector3, ScaleTween>(
            new Vector3(1, 1, 1), .3f, easing: BackEasing.Out);
    }
}
