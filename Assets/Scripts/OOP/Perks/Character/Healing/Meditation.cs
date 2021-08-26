using IgnitedBox.Utilities;
using Scripts.OOP.Perks.Character.Triggers;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.Stats;
using UnityEngine;

namespace Scripts.OOP.Perks.Character.Healing
{
    public class Meditation : Perk, IControllerUpdate, IReceiveDamage, IWeaponFire
    {
        protected override int ToBuffCharge => 50;

        protected override string GetDescription()
            => $"After not firing and not receiving damage for {cooldown} seconds, regenerate {Intensity} health per seconds";

        private float cooldown = 30;

        private float time = 30;

        protected override void Rebuild()
        {
            cooldown = Mathf.Floor(15 - Graphs.BoxedExponent(Intensity, 60, 10, 0.86f));
            base.Rebuild();
        }

        public bool OnControllerUpdate(BaseController controller, float delta)
        {
            if(time > 0)
            {
                time -= delta;
                return false;
            }

            controller.stats.health += delta * Intensity;
            return true;
        }

        public bool OnFire(float angle, Weapon weapon, WeaponStats buff)
        {
            time = cooldown;
            return false;
        }

        public bool OnHit(BaseController self, Vector2? direction, ref float damage)
        {
            time = cooldown;
            return false;
        }
    }
}
