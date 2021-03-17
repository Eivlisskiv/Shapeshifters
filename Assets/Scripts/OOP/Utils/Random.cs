using System;
using System.Collections.Generic;

namespace Scripts.OOP.Utils
{
    public static class Randomf
    {
        private static readonly Random rng = new Random();

        public static bool Chance(int chance)
            => chance > 0 && rng.Next(101) <= chance;

        public static T Element<T>(params T[] items)
            => items[rng.Next(items.Length)];
        public static T Element<T>(List<T> items)
            => items[rng.Next(items.Count)];
    }
}
