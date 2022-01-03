using IgnitedBox.Tweening.Tweeners;
using IgnitedBox.Tweening.Tweeners.FloatTweeners;
using System;
using UnityEngine.UI;

namespace Tween.Source.Tweeners.FloatTweeners
{
    public class TextFloatTweener : FloatTweener<Text>
    {
        public TextFloatTweener() { }

        public TextFloatTweener(Text e, float target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(e, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<Text, float> with)
        {
            Target = with.Target;
        }

        protected override float GetStart()
        {
            float.TryParse(Element.text, out float number);
            return number;
        }

        protected string ToText(float value, int precision)
            => Math.Round(value, precision).ToString();

        protected override void OnMove(float current)
            => Element.text = ToText(current, 2);
    }

    public class TextIntegerTweener : TextFloatTweener
    {
        public TextIntegerTweener() { }

        public TextIntegerTweener(Text e, float target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(e, target, time, delay, easing, callback) { }

        protected override void OnMove(float current)
            => Element.text = ToText(current, 0);
    }
}
