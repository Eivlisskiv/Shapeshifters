﻿using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks.Character.Triggers;
using System;
using UnityEngine;

namespace Scripts.OOP.Perks.Character
{
    public class Shield : EmitterPerk, IControllerUpdate, IProjectileTaken, IReceiveDamage
    {
        private float shield = 0;
        private float cooldown = 0;

        protected override int ToBuffCharge => 25;

        protected override string RessourcePath => "Particles/Shields";

        protected override string GetDescription()
            => $"Grants {Stat(Intensity * 5)} Shields with a 5 seconds recharge delay.";

        private bool Remaining => Charge > 0 || shield > 0;

        private bool noCharge = false;
        private float noChargeValue;

        public bool OnHit(BaseController self, ProjectileHandler projectile)
        {
            cooldown = 5;

            if (!projectile || shield <= 0) return !Remaining;

            Transform debrisParent = GameModes.GetDebrisTransform(self.Team);

            if(shield >= projectile.damage)
            {
                ShieldHealth(-projectile.damage, self);
                Shielded(projectile.transform.position, debrisParent);
                projectile.active = false;

                return !Remaining;
            }

            projectile.damage -= shield;
            ShieldHealth(-shield, self);
            Shielded(projectile.transform.position, debrisParent);

            return !Remaining;
        }

        public bool OnHit(BaseController self, Vector2? attackPosition, ref float damage)
        {
            cooldown = 5;

            if (damage <= 0 || shield <= 0) return !Remaining;

            Transform debrisParent = GameModes.GetDebrisTransform(self.Team);

            float reduction = Math.Min(damage, shield);

            damage -= reduction;
            ShieldHealth(-reduction, self);
            Shielded(attackPosition ?? self.transform.position, debrisParent);

            return !Remaining;
        }

        private void ShieldHealth(float amount, BaseController controller)
        {
            shield += amount;

            if (controller.HealthBar == null) return;

            controller.HealthBar.SetShield(Math.Min(1,
                shield / controller.stats.MaxHealth));
        }

        private void Shielded(Vector3 pos, Transform debrisParent)
        {
            pos.z = -5;
            UnityEngine.Object.Destroy(SpawnPrefab(pos, null, debrisParent), 1f);
        }

        public bool OnControllerUpdate(BaseController controller, float delta)
        {
            if (Level == 0 && noCharge && shield > 0)
            {
                if(noChargeValue >= Charge)
                    return false;

                noCharge = false;
            }

            if (cooldown > 0)
            {
                cooldown -= delta;
                if(cooldown > 0) return false;
            }

            float max = Intensity * 5;
            if (shield >= max) return false;

            ShieldHealth(Math.Min(max - shield, delta), controller);

            if(Level == 0 && Charge - delta <= 0)
            {
                noChargeValue = Charge;
                noCharge = true;
                return false;
            }

            return true;
        }
    }
}
