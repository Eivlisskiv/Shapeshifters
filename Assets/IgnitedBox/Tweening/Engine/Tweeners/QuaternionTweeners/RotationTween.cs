using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.QuaternionTweeners
{
    class RotationTween //: QuaternionTweener<Transform>
    {
        public RotationTween() { }

        /*
        public RotationTween(Transform element, Quaternion target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }


        public override void Blend(TweenData<Transform, Quaternion> with)
        {
            Duration = Math.Max(Duration, with.Duration);
            SetPositions(Element.rotation, Target * with.Target);
        }

        protected override Quaternion GetStart()
            => Element.rotation;

        protected override void OnFinish()
            => Element.rotation = Target;

        protected override void OnMove(Quaternion current)
            => Element.rotation = current;
        */
    }
}
