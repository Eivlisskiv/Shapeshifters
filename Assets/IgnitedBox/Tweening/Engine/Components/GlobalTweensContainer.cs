using IgnitedBox.Tweening.Tweeners;
using System.Collections.Generic;
using UnityEngine;

namespace IgnitedBox.Tweening.Components
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
                tweens[i].Update(tweens[i].scaledTime ? Time.deltaTime : Time.unscaledDeltaTime);
                if (tweens[i].State == TweenerBase.TweenState.Finished)
                    tweens.RemoveAt(i);
                else i++;
            }
        }

        public void Add<S, T>(TweenData<S, T> tween)
            => tweens.Add(tween);

        public void Dispose(TweenerBase element)
            => tweens.Remove(element);
    }
}
