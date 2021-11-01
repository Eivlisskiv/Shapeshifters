using System;
using Tween.Source.Tweeners.FloatTweeners;
using UnityEngine.UI;

namespace IgnitedBox.Tweening.TweenPresets
{
    public static class TextPresets
    {
        /// <summary>
        /// Tween a whole number.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="number"></param>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        /// <param name="easing"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static TextIntegerTweener TweenInteger(this Text text,
            int number, float time, float delay = 0, Func<double, double> easing = null,
            Action callback = null) => TweenHandling.Tween<Text, float, TextIntegerTweener>
            (text, number, time, delay, easing, callback);

        /// <summary>
        /// Tween a number with decimal 2 precision.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="number"></param>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        /// <param name="easing"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static TextFloatTweener TweenNumber(this Text text,
            float number, float time, float delay = 0, Func<double, double> easing = null,
            Action callback = null) => TweenHandling.Tween<Text, float, TextFloatTweener>
            (text, number, time, delay, easing, callback);
    }
}
