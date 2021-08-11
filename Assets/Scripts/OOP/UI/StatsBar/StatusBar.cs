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

        float target;
        bool healing;

        bool done;

        public StatusBar(Transform container)
        {
            live = container.Find("Live").GetComponent<Image>();
            delayed = container.Find("Delay").GetComponent<Image>();
        }

        public void SetValue(float v)
        {
            if (v == target) return;

            healing = v > target;
            target = v;

            if (healing) TweenChange(delayed, live);
            else TweenChange(live, delayed);
        }

        private void TweenChange(Image to, Image from)
        {
            to.fillAmount = target;
            TweenFill(from, target, 2.5f);
        }

        protected void TweenFill(Image img, float percent, float time)
        {
            float change = Mathf.Abs(img.fillAmount - percent);
            if(change < 0.01)
            {
                img.fillAmount = percent;
                return;
            }

            img.Tween<Image, float, ImageFillTween>(percent, time, easing: ExponentEasing.Out);
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
