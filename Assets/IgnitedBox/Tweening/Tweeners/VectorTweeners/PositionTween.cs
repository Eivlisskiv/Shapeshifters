using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.VectorTweeners
{
    public class PositionTween : VectorTweener<Transform>
    {
        public PositionTween() { }
        public PositionTween(Transform element, Vector3 target, float time, 
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<Transform, Vector3> with)
        {
            Duration = Math.Max(Duration, with.Duration);
            SetPositions(Subject.position, Target + with.Target);
        }

        protected override Vector3 GetStart()
            => Subject.localPosition;

        protected override void OnFinish()
            => Subject.localPosition = Target;

        protected override void OnMove(Vector3 current)
            => Subject.localPosition = current;
    }
}
