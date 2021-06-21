using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.VectorTweeners
{
    [Serializable]
    public class ScaleTween : VectorTweener<Transform>
    {
        public ScaleTween() { }
        public ScaleTween(Transform element, Vector3 target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<Transform, Vector3> with)
        { }

        protected override Vector3 GetStart()
            => Element.localScale;

        protected override void OnFinish()
            => Element.localScale = Target;

        protected override void OnMove(Vector3 current)
            => Element.localScale = current;
    }
}
