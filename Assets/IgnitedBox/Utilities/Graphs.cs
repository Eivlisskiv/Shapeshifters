using UnityEngine;

namespace IgnitedBox.Utilities
{
    public static class Graphs
    {
        public static float BoxedExponent(float x, float width, float height, float curve)
        {
            //https://www.desmos.com/calculator/i7yvbk6o2k
            float a = height / (Mathf.Pow(curve, width) - 1);
            return a * Mathf.Pow(curve, x) - a;
        }
    }
}
