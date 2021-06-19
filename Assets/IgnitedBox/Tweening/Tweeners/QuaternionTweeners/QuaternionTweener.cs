using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.QuaternionTweeners
{
    public abstract class QuaternionTweener<T> //: TweenData<T, Quaternion>
    {
        protected QuaternionTweener() { }

        /*

        protected QuaternionTweener(T element, Quaternion target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        protected override Quaternion GetTween()
            => Target * Quaternion.Inverse(Start);

        public override Quaternion GetTweenAt(float percent)
            => Start * (Tween * percent);
        */
    }
}
