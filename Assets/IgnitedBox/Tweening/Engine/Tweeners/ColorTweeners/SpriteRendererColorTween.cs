using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.ColorTweeners
{
    class SpriteRendererColorTween : ColorTweener<SpriteRenderer>
    {
        public SpriteRendererColorTween() { }

        public SpriteRendererColorTween(SpriteRenderer element, Color target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        protected override Color GetStart()
            => Element.color;

        protected override void OnMove(Color current)
            => Element.color = current;
    }
}
