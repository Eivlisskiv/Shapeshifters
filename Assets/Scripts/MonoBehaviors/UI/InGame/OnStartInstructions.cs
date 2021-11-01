using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OnStartInstructions : MonoBehaviour
{
    public Text front;
    public Text back;

    public Action onReady;

    public Image a;
    public Image b;

    bool dying = false;

    private void Start()
    {
        a.transform.localScale = Vector3.one;
        a.transform.Tween<Transform, Vector3, ScaleTween>(
            Vector3.zero, 0.5f, 0, ElasticEasing.InOut)
            .loop = IgnitedBox.Tweening.Tweeners.TweenerBase.LoopType.ReverseLoop;
        b.transform.localScale = Vector3.zero;
        b.transform.Tween<Transform, Vector3, ScaleTween>(
            Vector3.one, 0.5f, 0, ElasticEasing.InOut)
            .loop = IgnitedBox.Tweening.Tweeners.TweenerBase.LoopType.ReverseLoop;
    }

    // Update is called once per frame
    void Update() 
    {
        if (!dying && Input.GetMouseButton(0))
        {
            dying = true;

            transform.Tween<Transform, Vector3, ScaleTween>
                (new Vector3(0, 0, 0), 0.8f, 1, BackEasing.In,
                Callback).scaledTime = false;
        }
    }

    public void Callback()
    {
        if (onReady != null) onReady();
        Destroy(gameObject);
    }

    public void SetObjective(string text)
    {
        front.text = text;
        back.text = text;
    }
}
