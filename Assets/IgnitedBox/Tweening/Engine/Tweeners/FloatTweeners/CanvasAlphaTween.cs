using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.FloatTweeners
{
    public class CanvasAlphaTween : FloatTweener<CanvasGroup>
    {
        public CanvasAlphaTween() { }

        public CanvasAlphaTween(CanvasGroup element, float target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<CanvasGroup, float> with)
        {
            Target = with.Target;
        }

        protected override float GetStart()
            => Element.alpha;

        protected override void OnMove(float current)
            => Element.alpha = current;

    }
}
