using IgnitedBox.EditorDropdown.ByAttribute;
using Scripts.OOP.Perks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors
{
    [Serializable]
    public class EnemySettings
    {
        public List<Type> perkslist = PerksHandler.types;

        public string name;

        public int baseHealth;
        public int bonusHealth;

        public Color color;

        public bool randomCorners;

        [Range(3, 10)]
        public int corners;

        [Range(1, 5)]
        public int size;

        [Dropdown(typeof(EnemyBehavior), "targets")]
        public string targettingBehavior;

        [Dropdown(typeof(EnemyBehavior), "firing")]
        public string firingBehavior;

        [Dropdown(typeof(PerksHandler), "types")]
        public string[] perk;

        public string[] perks;

        public EnemyBehavior SetSettings(EnemyController self)
        {
            self.gameObject.name = name;

            self.body.corners = randomCorners ?
            UnityEngine.Random.Range(3, 11) : corners;
            self.body.radius = size;
            self.stats = new Character.Stats.Stats(baseHealth +
                (self.Level * bonusHealth));

            self.SetColor(1, color);

            SetPerks(self);

            return new EnemyBehavior(targettingBehavior, firingBehavior);
        }

        private void SetPerks(EnemyController self)
        {
            self.perks = new PerksHandler();

            if(perks == null || perks.Length == 0)
            {
                RandomPerks(self);
                return;
            }

            int level = self.Level / perks.Length;
            int charge = self.Level;

            for (int i = 0; i < perks.Length; i++)
            {
                string name = perks[i];
                Perk perk = PerksHandler.Load(name);
                if (perk == null) continue;

                perk.LevelUp(level);
                self.perks.Add(perk);
                charge -= level;
            }

            if (charge > 0)
                RandomPerk(charge, self);
        }

        private void RandomPerk(int level, EnemyController self)
        {
            Perk perk = PerksHandler.Random();
            perk.LevelUp(level);
            self.perks.Add(perk);
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
