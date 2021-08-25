using IgnitedBox.Tweening.TweenPresets;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability.Cloning
{
    public class Number_Four : EnemyCloner
    {
        public Number_Four() : base("Boss/Number Four", true, 5) { }

        public override void Initialize(BaseController self)
        {
            self.Events.Subscribe<BaseController, BaseController>
                (BaseController.ControllerEvents.DamageTaken, DamageTaken);
        }

        public void DamageTaken(BaseController self, BaseController attacker)
        {
            if (!(self is EnemyController parent)) return;
            if (spawns >= maxSpawns) return;
            float next = 1f - ((1f / maxSpawns) * (spawns + 1));
            if (self.stats.HPP > 0 && self.stats.HPP <= next)
            {
                BaseController clone = Clone(parent);

                clone.transform.localScale = new Vector3(0, 0, 0);
                clone.transform.TweenScale(new Vector3(1, 1, 1), 0.25f);
            }
        }
    }
}