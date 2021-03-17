using Scripts.OOP.Stats;

namespace Scripts.OOP.Perks.Weapon
{
    public class Charging_Round : Perk, IProjectileHitTarget
    {
        protected override string GetDescription()
            => $"Projectiles air time increase projectile force by 100% and damage by ({(Intensity * 10)}%).";

        public bool OnHit(ProjectileHandler projectile, BaseController target)
        {
            projectile.damage *= 1 + (projectile.airtime * (Intensity / 10f));
            projectile.force += projectile.airtime;
            return true;
        }
    }

    public class Barrel_Buff : Perk, IWeaponFire
    {
        protected override string GetDescription()
            => $"Increases firing radius and damage by ({Intensity}).";

        public bool OnFire(float _, WeaponHandler weapon, WeaponStats buff)
        {
            buff.angle += Intensity;
            buff.totalDamage += Intensity;
            return true;
        }
    }
}
