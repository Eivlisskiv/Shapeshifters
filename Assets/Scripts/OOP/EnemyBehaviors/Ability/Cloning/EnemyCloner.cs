using IgnitedBox.Tweening.TweenPresets;
using Scripts.OOP.Game_Modes;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability.Cloning
{
    public abstract class EnemyCloner : IAbilitybehavior
    {
        private readonly string path;
        private readonly List<BaseController> clones = new List<BaseController>();

        public readonly int maxSpawns;

        public int spawns { get;  private set; }

        private readonly bool areIndependant;

        public EnemyCloner(string path, bool areIndependant, int spawns = 3)
        {
            maxSpawns = spawns;
            this.areIndependant = areIndependant;
            this.path = $"Enemies/{path}";
        }

        public abstract void Initialize(BaseController self);

        protected EnemyController Clone(BaseController parent)
        {
            int size = System.Math.Max(1, parent.Body.Radius - 1);

            GameObject character = Resources.Load<GameObject>(path);
            if (!character) return null;

            character = Object.Instantiate(character);
            EnemyController clone = character.GetComponent<EnemyController>();

            clones.Add(clone);

            clone.settings.size = size;
            clone.settings.baseHealth = (int)(parent.stats.HPP * clone.settings.baseHealth);

            clone.Set(System.Math.Max(1, parent.Level - 1));

            if (GameModes.GameMode != null)
            {
                GameModes.GameMode.AddMember(parent.Team, clone, true);
                GameModes.GameMode.RandomMapPosition(clone.transform);
            }
            else
            {
                clone.transform.position = parent.transform.position
                + new Vector3(0, (size * 2) + 2);

                clone.Team = parent.Team;
                clone.SetColor(0, parent.GetColor(0));
                clone.settings.targettingBehavior = "TestPlayerTarget";
            }

            spawns++;

            return clone;
        }
    }
}
