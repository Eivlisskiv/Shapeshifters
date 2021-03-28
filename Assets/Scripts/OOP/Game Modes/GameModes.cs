using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Game_Modes
{
    public static class GameModes
    {
        public static AGameMode GameMode
        { get => _instance; }

        private static AGameMode _instance;

        internal static void SetInstance(AGameMode aGameMode)
            => _instance = aGameMode;

        public static readonly Dictionary<Type, string> modes = new Dictionary<Type, string>()
        {
            {
                typeof(Arena.Arena),
                "Survive enemy spawns." +
                "\n Gain a random perk every 5 levels." +
                "\n Gain a temporary buff when elimenating an enemy."
            },

            {
                typeof(Rogue.Rogue),
                "Go through rooms of enemies. " +
                "\n Purchase perks after each room."
            }
        };

        public static void Run<T>(Action<T> func)
        {
            if (_instance is T mode) func(mode);
        }

        public static Transform GetDebrisTransform(int team)
            => _instance?.teamTransforms[team].debris;

        public static int LoadScore(Type mode = null)
        {
            string name = mode?.Name.Replace('_', ' ') ?? _instance.Name;
            return PlayerPrefs.GetInt($"Score_{name}");
        }

        public static void SaveScore(int score)
        {
            PlayerPrefs.SetInt($"Score_{_instance.Name}", score);
        }
    }
}
