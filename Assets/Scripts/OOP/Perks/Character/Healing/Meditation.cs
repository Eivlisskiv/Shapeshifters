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
            => $"After not receiving damage for {cooldown} seconds, regenerate {Intensity / 2f} health per seconds." +
            $" Healing doubled when also not firing for {cooldown} seconds.";

        private float cooldown = 30;

        private float damageCooldown = 30;
        private float firingCooldown = 30;

        protected override void Rebuild()
        {
            cooldown = Mathf.Floor(15 - Graphs.BoxedExponent(Intensity, 60, 10, 0.86f));
            base.Rebuild();
        }

        public bool OnControllerUpdate(BaseController controller, float delta)
        {
            if(damageCooldown > 0)
            {
                damageCooldown -= delta;
                return false;
            }

            bool bonus = true;
            if (firingCooldown > 0)
            {
                firingCooldown -= delta;
                bonus = false;
            }

            controller.ModifyHealth(delta * Intensity * (bonus ? 1 : 0.5f));
            return true;
        }

        public bool OnFire(float angle, Weapon weapon, WeaponStats buff)
        {
            firingCooldown = cooldown;
            return false;
        }

        public bool OnHit(BaseController self, Vector2? direction, ref float damage)
        {
            damageCooldown = cooldown;
            return false;
        }
    }
}
