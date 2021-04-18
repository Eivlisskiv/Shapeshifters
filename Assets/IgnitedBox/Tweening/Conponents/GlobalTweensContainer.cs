using IgnitedBox.Tweening.Tweeners;
using System.Collections.Generic;
using UnityEngine;

namespace IgnitedBox.Tweening.Conponents
{
    public class GlobalTweensContainer : MonoBehaviour
    {
        private readonly List<TweenerBase> tweens
            = new List<TweenerBase>();

        void Update()
            => TweenTransforms();

        private void TweenTransforms()
        {
            if (tweens.Count == 0) return;
            int i = 0;
            while (i < tweens.Count)
            {
                TweenerBase tween = tweens[i];
                if (tween.Update(Time.deltaTime)) tweens.RemoveAt(i);
                else i++;
            }
        }

        public void Add<S, T>(TweenData<S, T> tween)
            => tweens.Add(tween);
    }
}
