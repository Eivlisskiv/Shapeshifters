using System;

namespace IgnitedBox.Tweening.EasingFunctions
{
    public static class SineEasing
    {
        public static double In(double x)
            => 1 - Math.Cos(x * Math.PI / 2f);
        public static double Out(double x)
            => Math.Sin(x * Math.PI / 2f);
        public static double InOut(double x)
            => -(Math.Cos(Math.PI * x) - 1) / 2;
    }
}
