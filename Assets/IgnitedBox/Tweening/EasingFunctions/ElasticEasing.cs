using System;

namespace IgnitedBox.Tweening.EasingFunctions
{
    public static class ElasticEasing
    {
        private static bool DoEase(double x, out double c)
        {
            c = (2 * Math.PI) / 3;
            return x != 0 && x != 1;
        }

        public static double In(double x)
        {
            if (!DoEase(x, out double c)) return x;

            return -Math.Pow(2, 10 * x - 10)
                * Math.Sin((x * 10 - 10.75) * c);
        }

        public static double Out(double x)
        {
            if (!DoEase(x, out double c)) return x;
            return Math.Pow(2, -10 * x)
                * Math.Sin((x * 10 - 0.75) * c) + 1;
        }

        public static double InOut(double x)
        {
            double c = (2 * Math.PI) / 4.5;

            if (x == 0 || x == 1) return x;

            double a = Math.Sin((20 * x - 11.125) * c);
            return x < 0.5 ?
                -(Math.Pow(2, 20 * x - 10) * (a / 2))
                : Math.Pow(2, -20 * x + 10) * a / 2 + 1;
        }
    }
}
