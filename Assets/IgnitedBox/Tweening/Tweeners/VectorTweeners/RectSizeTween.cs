using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.VectorTweeners
{
    public class RectSizeTween : VectorTweener<RectTransform>
    {
        public RectSizeTween() { }

        public RectSizeTween(RectTransform element, Vector3 target, float time, 
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<RectTransform, Vector3> with)
        {
            Duration = Math.Max(Duration, with.Duration);
            Target += with.Target;
        }

        protected override Vector3 GetStart()
            => Subject.sizeDelta;

        protected override void OnFinish()
            => Subject.sizeDelta = Target;

        protected override void OnMove(Vector3 current)
            => Subject.sizeDelta = current;
    }
}
