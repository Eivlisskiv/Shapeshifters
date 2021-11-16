using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using Scripts.OOP.UI;
using System;
using UnityEngine;

public class ObjectivePointer : MonoBehaviour
{
    static Color gold = new Color(255/255f, 192/255f, 61/255f, 1);

    public SpriteRenderer main;
    public SpriteRenderer sub;

    ObjectiveElement.ObjectiveTracking? target;

    private void Start()
    {
        StartTracking();
    }

    private void Update()
    {
        if (!isActiveAndEnabled || !target.HasValue) return;

        transform.LookAt((Vector3)target);
    }

    public void Subscribe(ObjectiveHandler handler)
    {
        handler.Events.Subscribe<ObjectiveHandler, ObjectiveElement>
            (ObjectiveHandler.ObjectiveEvents.Created, Subscribe);
    }

    private void Subscribe(ObjectiveHandler handler, ObjectiveElement element)
    {
        Track(null);
        element.Events.Subscribe<ObjectiveElement, ObjectiveElement.ObjectiveTracking?>
            (ObjectiveHandler.ObjectiveEvents.TrackModified, (e, t) =>
            {
                if (e == null || handler.Current != e || !t.HasValue) return;
                Track(t);
            });
    }

    private void Track(ObjectiveElement.ObjectiveTracking? track)
    {
        if (!track.HasValue)
        {
            if (target.HasValue) EndTracking();
            target = null;
            return;
        }

        target = track;
        StartTracking();
    }

    private void StartTracking()
    {
        enabled = true;

        main.UnscaledTween<SpriteRenderer, Color, SpriteRendererColorTween>
            (gold, 0.2f);

        StartSub(sub);
        //StartSub(sub2, 0.5f);
    }

    private void StartSub(SpriteRenderer sub)
    {
        sub.color = gold;
        sub.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        sub.transform.UnscaledTween<Transform, Vector3, ScaleTween>
            (new Vector3(1.5f, 1.5f, 1), 0.5f, 0.5f, callback: () => 
            {
                sub.UnscaledTween<SpriteRenderer, Color, SpriteRendererColorTween>
                    (Color.clear, 1f);

            }).loop = IgnitedBox.Tweening.Tweeners.TweenerBase.LoopType.ResetLoop;
    }

    private void EndTracking()
    {
        enabled = false;

        main.UnscaledTween<SpriteRenderer, Color, SpriteRendererColorTween>
            (Color.clear, 0.2f);

        HideSub(sub);
    }

    private void HideSub(SpriteRenderer sub)
    {
        sub.UnscaledTween<SpriteRenderer, Color, SpriteRendererColorTween>
            (Color.clear, 0.2f);

        sub.transform.UnscaledTween<Transform, Vector3, ScaleTween>
            (Vector3.zero, 0.25f);
    }
}
