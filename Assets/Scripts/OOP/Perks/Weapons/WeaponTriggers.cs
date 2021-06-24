using Scripts.OOP.Stats;

namespace Scripts.OOP.Perks.Weapons
{
    public interface IProjectileHitTarget
    {
        bool OnHit(ProjectileHandler projectile, BaseController target);
    }

    public interface IProjectileFired
    {
        bool OnFire(ProjectileHandler projectile);
    }

    public interface IWeaponFire
    {
        bool OnFire(float angle, Weapon weapon, WeaponStats buff);
    }
}