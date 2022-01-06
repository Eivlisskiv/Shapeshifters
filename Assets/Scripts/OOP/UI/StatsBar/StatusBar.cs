using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.FloatTweeners;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.UI.StatsBar
{
    public class StatusBar
    {
        readonly Image live;
        readonly Image delayed;

        private ImageFillTween liveTween;
        private ImageFillTween delayedTween;

        float target;
        bool healing;

        bool done;

        public StatusBar(Transform container)
        {
            live = container.Find("Live").GetComponent<Image>();
            delayed = container.Find("Delay").GetComponent<Image>();
        }

        public void SetHealth(float v)
        {
            if (v == target) return;

            healing = v > target;
            target = v;

            liveTween?.Dispose();
            delayedTween?.Dispose();

            if (healing) liveTween = TweenChange(delayed, live);
            else delayedTween = TweenChange(live, delayed);
        }

        private ImageFillTween TweenChange(Image to, Image from)
        {
            to.fillAmount = target;
            return TweenFill(from, target, 2.5f);
        }

        protected ImageFillTween TweenFill(Image img, float percent, float time)
        {
            float change = Mathf.Abs(img.fillAmount - percent);
            if(change < 0.01)
            {
                img.fillAmount = percent;
                return null;
            }

            return img.Tween<Image, float, ImageFillTween>(percent, 
                IgnitedBox.Tweening.Components.Tweener.BlendType.Replace,
                time, easing: ExponentEasing.Out);
        }

        public void Update(float v)
        {
            if (v != target)
            {
                healing = v > target;
                target = v;
                done = false;
            }
            else if (done) return;

            if (healing) UpdateDirection(delayed, live);
            else UpdateDirection(live, delayed);
        }

        private void UpdateDirection(Image to, Image from)
        {
            float tov = to.fillAmount;
            if(tov != target) to.fillAmount = target;
            float fromv = from.fillAmount;
            if (fromv != target)
            {
                if (Mathf.Abs(target - fromv) < 0.01)
                {
                    from.fillAmount = target;
                    done = true;
                }
                else from.fillAmount = Mathf.Lerp(fromv, target, Time.deltaTime);
            }
        }
    }
}
