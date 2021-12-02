using UnityEngine;

namespace IgnitedBox.Utilities
{
    public static class Graphs
    {
        public static int TierRange(int tiers, float curve)
            => Mathf.FloorToInt(BoxedExponent(UnityEngine.Random.Range(0, 101), 100, tiers, curve));

        public static float BoxedExponent(float x, float width, float height, float curve)
        {
            //https://www.desmos.com/calculator/i7yvbk6o2k
            float a = height / (Mathf.Pow(curve, width) - 1);
            return a * Mathf.Pow(curve, x) - a;
        }

        public static float BoxedExponentInverse(float x, float width, float height, float curve)
        {
            //https://www.desmos.com/calculator/wcruhkv5yf
            float a = x * (Mathf.Pow(curve, width) - 1);
            a = Mathf.Log((a + height) / height);
            return a / Mathf.Log(curve);
        }

        public static float LimitedGrowthExponent(float x, float limit, float smoothness, float start = 0)
        {
            return -limit * Mathf.Pow(smoothness, x) + limit + start;
        }
    }
}
