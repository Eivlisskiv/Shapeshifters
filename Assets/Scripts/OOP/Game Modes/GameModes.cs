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

        public static readonly Dictionary<Type, ModeData> arcadeModes = new Dictionary<Type, ModeData>()
        {
            {
                typeof(Arena.Arena),
                new ModeData()
                {
                    description = " Survive enemy spawns." +
                    "\n Gain a random perk every 5 levels." +
                    "\n Gain a temporary buff when" +
                    "\n elimenating an enemy." +
                    "\n Your weapon randomly switches every 5 eliminations",
                    storyRequirement = ( 1, 3 )
                }
            },

            {
                typeof(Rogue.Rogue),
                new ModeData()
                {
                    description =" Go through rooms of enemies. " +
                    "\n Purchase perks after each room.",
                    storyRequirement = ( 1, 3 )
                }
            }
        };

        public class ModeData
        {
            public string description;
            public (int, int) storyRequirement;
        }

        public static void Run<T>(Action<T> func)
        {
            if (_instance is T mode) func(mode);
        }

        public static Transform GetDebrisTransform(int team)
            => _instance?.teamTransforms[team].debris;
    }
}
