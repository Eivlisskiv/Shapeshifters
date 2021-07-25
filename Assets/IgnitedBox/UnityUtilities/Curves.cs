using System;
using UnityEngine;

namespace Assets.IgnitedBox.UnityUtilities
{
    public static class Curves
    {
        public static AnimationCurve ToCurve(Func<float, float> function,
            int frameCount = 10)
        {
            AnimationCurve curve = new AnimationCurve();
            float sep = 1f / (frameCount - 1);
            for(int i = 0; i < frameCount; i ++)
            {
                float time = i * sep;
                curve.AddKey(time, function(time));
                curve.SmoothTangents(i, 1);
            }

            return curve;
        }
    }
}
