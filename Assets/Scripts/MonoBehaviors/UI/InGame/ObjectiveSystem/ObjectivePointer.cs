using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using Scripts.OOP.UI;
using Scripts.OOP.Utils;
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
        main.color = Color.clear;
        sub.color = Color.clear;
        Subscribe(ObjectiveHandler.Instance);
    }

    private void Update()
    {
        if (!isActiveAndEnabled || !target.HasValue) return;

        float a2 = ((Vector2)transform.position).WorldAngle((Vector2)target);
        transform.rotation = Quaternion.Euler(0, 0, a2);
    }

    public void Subscribe(ObjectiveHandler handler)
    {
        handler.Events.Subscribe<ObjectiveHandler, ObjectiveElement>
            (ObjectiveHandler.ObjectiveEvents.Created, (a, b) => CheckTrack(handler.Current));

        handler.Events.Subscribe<ObjectiveElement, ObjectiveElement.ObjectiveTracking?>
            (ObjectiveHandler.ObjectiveEvents.TrackModified, (a, b) => CheckTrack(handler.Current));

        handler.Events.Subscribe<ObjectiveHandler, ObjectiveElement>
            (ObjectiveHandler.ObjectiveEvents.Removed, (a, b) => CheckTrack(handler.Current));

    }

    private void CheckTrack(ObjectiveElement element)
    {
        Track(element == null || !element.Track.HasValue ? 
            null : element.Track);
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
    }

    private void StartSub(SpriteRenderer sub)
    {
        sub.color = Color.clear;
        sub.transform.localScale = new Vector3(0.8f, 0.8f, 1);

        const float anim_time = 1.5f;
        const float anim_delay = 1;

        sub.transform.UnscaledTween<Transform, Vector3, ScaleTween>
            (new Vector3(1.3f, 1.3f, 1), anim_time, anim_delay, easing: SizeEasing)
            .loop = IgnitedBox.Tweening.Tweeners.TweenerBase.LoopType.ResetLoop;

        sub.UnscaledTween<SpriteRenderer, Color, SpriteRendererColorTween>
            (gold, anim_time, anim_delay, easing: ColorEasing, callback: () => sub.color = Color.clear)
            .loop = IgnitedBox.Tweening.Tweeners.TweenerBase.LoopType.ResetLoop;
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

    private double SizeEasing(double t)
    {
        if (t >= 0.75) return 1;

        return 0.5 + (Math.Sin( (4 * Math.PI * t) + (1.5 * Math.PI) ) / 2);
    }

    private double ColorEasing(double t)
    {
        if (t < 0.25) return (16 * Math.Pow(t, 2));
        else if (t < 0.75) return (16 * Math.Pow(t - 0.5, 2));
        return 16 * Math.Pow(t - 1, 2);
    }
}
