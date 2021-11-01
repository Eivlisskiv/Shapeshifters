using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using System;
using UnityEngine;

namespace IgnitedBox.Tweening.TweenPresets
{
    public static class GraphicPresets
    {
        public static GraphicColorTween TweenColor(this UnityEngine.UI.Graphic graphics,
            Color color, float time, float delay = 0, Func<double, double> easing = null,
            Action callback = null) => TweenHandling.Tween<UnityEngine.UI.Graphic, Color, GraphicColorTween>
            (graphics, color, time, delay, easing, callback);
    }
}
