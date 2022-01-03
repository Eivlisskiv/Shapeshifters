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

        protected override Color GetStart()
            => Element.color;

        protected override void OnMove(Color current)
            => Element.color = current;
    }
}
