using Scripts.OOP.Stats;

namespace Scripts.OOP.Perks.Weapons
{
    public class Charging_Round : Perk, IProjectileHitTarget
    {
        protected override int ToBuffCharge => 2;

        protected override string GetDescription()
            => $"Projectiles air time increase projectile force by 100% and damage by ({(Intensity * 10)}%).";

        public bool OnHit(ProjectileHandler projectile, BaseController target)
        {
            projectile.damage *= 1 + (projectile.Airtime * (Intensity / 10f));
            projectile.force += projectile.Airtime;
            return true;
        }
    }

    public class Barrel_Buff : Perk, IWeaponFire
    {
        protected override string GetDescription()
            => $"Increases projectile damage by ({Intensity}).";

        public bool OnFire(float _, Weapon weapon, WeaponStats buff)
        {
            buff.totalDamage += Intensity;
            return true;
        }
    }
}
