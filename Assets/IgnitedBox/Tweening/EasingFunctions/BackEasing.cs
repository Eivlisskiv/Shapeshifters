using System;

namespace IgnitedBox.Tweening.EasingFunctions
{
    public static class BackEasing
    {
        public static double Out(double x)
        {
            double a = 1.70158;
            double b = 1 + a;

            return 1 + b * Math.Pow(x - 1, 3) + a * Math.Pow(x - 1, 2);
        }
    }
}
