using System;

namespace IgnitedBox.Tweening.EasingFunctions
{
    public static class BackEasing
    {
        const double a = 1.70158;

        public static double Out(double x)
        {
            double b = 1 + a;
            return 1 + b * Math.Pow(x - 1, 3) + a * Math.Pow(x - 1, 2);
        }

        public static double In(double x)
        {
            var b = a + 1;
            return b * x * x* x - a* x * x;
        }
    }
}
