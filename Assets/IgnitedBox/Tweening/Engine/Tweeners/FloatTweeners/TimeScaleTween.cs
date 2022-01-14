using System;

namespace IgnitedBox.Tweening.Tweeners.FloatTweeners
{
    public class TimeScaleTween : FloatTweener<float>
    {
        public TimeScaleTween() { }

        public TimeScaleTween(float element, float target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<float, float> with)
        {
            Target = with.Target;
        }

        protected override float GetStart()
            => UnityEngine.Time.timeScale;

        protected override void OnMove(float current)
            => UnityEngine.Time.timeScale = current;
    }
}
