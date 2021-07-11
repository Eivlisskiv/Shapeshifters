using System;
using UnityEngine;
using UnityEngine.UI;

namespace IgnitedBox.Tweening.Tweeners.ColorTweeners
{
    [Serializable]
    public class GraphicColorTween : ColorTweener<Graphic>
    {
        public GraphicColorTween() { }
        public GraphicColorTween(Graphic element, Color target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<Graphic, Color> with)
            => Target *= with.Target;

        public override Color GetTweenAt(float percent)
            => Start + (Tween * percent);

        protected override Color GetStart()
            => Element.color;

        protected override Color GetTween()
            => Target - Start;

        protected override void OnFinish()
            => Element.color = Target;

        protected override void OnMove(Color current)
            => Element.color = current;
    }
}
