using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.TweenPrefabs
{
    public static class GraphicTweens
    {
        public static GraphicColorTween TweenGraphicColor(this UnityEngine.UI.Graphic graphics,
            Color color, float time, float delay = 0, Func<double, double> easing = null,
            Action callback = null) => TweenHandling.Tween<UnityEngine.UI.Graphic, Color, GraphicColorTween>
            (graphics, color, time, delay, easing, callback);
    }
}
