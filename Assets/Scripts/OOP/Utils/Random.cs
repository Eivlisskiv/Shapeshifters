﻿using System;
using System.Collections.Generic;

namespace Scripts.OOP.Utils
{
    public static class Randomf
    {
        private static readonly Random rng = new Random();

        public static bool Chance(int chance)
            => chance > 0 && rng.Next(101) <= chance;

        public static bool Chance(float chance)
            => chance > 0 && (rng.Next(100) + rng.NextDouble()) <= chance;


        public static T Element<T>(params T[] items)
            => items[rng.Next(items.Length)];

        public static T RandomElement<T>(this T[] items)
            => items[rng.Next(items.Length)];

        public static T RandomElement<T>(this List<T> items)
            => items[rng.Next(items.Count)];

        internal static int Int(int v1, int v2)
            => rng.Next(v1, v2 + 1);
    }
}
