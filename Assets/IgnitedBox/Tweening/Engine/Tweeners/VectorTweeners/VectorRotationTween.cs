using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.VectorTweeners
{
    public class VectorRotationTween : VectorTweener<Transform>
    {
        public VectorRotationTween() { }
        public VectorRotationTween(Transform element, Vector3 target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<Transform, Vector3> with)
        {
            Duration = Math.Max(Duration, with.Duration);
            SetPositions(Element.position, Target + with.Target);
        }

        protected override Vector3 GetStart()
            => Element.localRotation.eulerAngles;

        protected override void OnFinish()
            => Element.localRotation = Quaternion.Euler(Target);

        protected override void OnMove(Vector3 current)
            => Element.localRotation = Quaternion.Euler(current);
    }
}
