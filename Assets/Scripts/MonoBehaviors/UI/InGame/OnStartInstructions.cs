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

    bool dying = false;

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
