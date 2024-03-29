﻿#if UNITY_EDITOR
using IgnitedBox.EditorDropdown.Attribute;
#endif
using Scripts.OOP.Perks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors
{
    [Serializable]
    public class EnemySettings
    {
        public static readonly Dictionary<string, string> miniBosses = new Dictionary<string, string>()
        {
            { "Pyramid", "Eye Holder" },
            { "Number Four", "Cloner" },
            { "PRDS", "RDS" },
        };

        public string name;

        public int baseHealth;
        public int bonusHealth;

        public Color color;

        public bool randomCorners;

        [Range(3, 10)]
        public int corners;

        [Range(1, 5)]
        public int size;

#if UNITY_EDITOR
        [Dropdown(typeof(EnemyBehavior), "targettingBehaviors.Keys")]
#endif
        public string targettingBehavior;
#if UNITY_EDITOR
        [Dropdown(typeof(EnemyBehavior), "firingBehaviors.Keys")]
#endif
        public string firingBehavior;
#if UNITY_EDITOR
        [Dropdown(typeof(EnemyBehavior), "abilityBehaviors.Keys")]
#endif
        public string abilityBehavior;

#if UNITY_EDITOR
        [Dropdown(typeof(PerksHandler), "perksTypes.Keys")]
#endif
        public string[] perks;

        public EnemyBehavior SetSettings(EnemyController self)
        {
            self.gameObject.name = name;

            self.Body.corners = randomCorners ?
            UnityEngine.Random.Range(3, 11) : corners;
            self.Body.Radius = size;
            self.stats = new Character.Stats.Stats(baseHealth +
                (self.Level * bonusHealth));

            self.SetColor(1, color);

            SetPerks(self);

            EnemyBehavior behavior = new EnemyBehavior(targettingBehavior, firingBehavior, abilityBehavior);
            behavior.ability.Initialize(self);
            return behavior;
        }

        private void SetPerks(EnemyController self)
        {
            self.perks = new PerksHandler();

            if(perks == null || perks.Length == 0)
            {
                RandomPerks(self);
                return;
            }

            int level = Math.Max(self.Level / perks.Length, 1);
            int charge = self.Level;

            for (int i = 0; i < perks.Length; i++)
            {
                string name = perks[i];
                Perk perk = PerksHandler.Load(name);
                if (perk == null) continue;

                perk.LevelUp(level);
                self.AddPerk(perk);
                charge -= level;
            }

            if (charge > 0)
                RandomPerk(charge, self);
        }

        private void RandomPerk(int level, EnemyController self)
        {
            Perk perk = PerksHandler.Random(level);
            self.AddPerk(perk);
        }

        private void RandomPerks(EnemyController self)
        {
            int charge = self.Level;

            while(charge > 0)
            {
                int level = charge > 10 ?
                    UnityEngine.Random.Range(
                        charge / 10, charge / 5)
                    : (charge > 5 ? UnityEngine.Random.Range(
                        1, 5) : 1);
                RandomPerk(level, self);
                charge -= level;
            }
        }
    }
}
