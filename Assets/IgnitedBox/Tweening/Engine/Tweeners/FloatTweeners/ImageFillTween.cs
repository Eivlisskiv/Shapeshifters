using System;
using UnityEngine.UI;

namespace IgnitedBox.Tweening.Tweeners.FloatTweeners
{
    public class ImageFillTween : FloatTweener<Image>
    {
        public ImageFillTween() { }

        public ImageFillTween(Image element, float target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<Image, float> with)
        {
            Duration = Math.Max(Duration, with.Duration);
            SetPositions(Element.fillAmount, Target + with.Target);
        }

        protected override float GetStart()
            => Element.fillAmount;

        protected override void OnMove(float current)
            => Element.fillAmount = current;
    }
}
