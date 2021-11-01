using System;

namespace IgnitedBox.Tweening.EasingFunctions
{
    public static class ExponentEasing
    {
        public static double Out(double x)
            => x == 1 ? 1 : 1 - Math.Pow(2, -10 * x);
    }
}
