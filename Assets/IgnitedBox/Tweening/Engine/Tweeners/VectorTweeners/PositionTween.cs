using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.VectorTweeners
{
    [Serializable]
    public class PositionTween : VectorTweener<Transform>
    {
        public PositionTween() { }
        public PositionTween(Transform element, Vector3 target, float time, 
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<Transform, Vector3> with)
        {
            Duration = Math.Max(Duration, with.Duration);
            SetPositions(Element.localPosition, Target + with.Target);
        }

        protected override Vector3 GetStart()
            => Element.localPosition;

        protected override void OnMove(Vector3 current)
            => Element.localPosition = current;
    }
}
