using Scripts.OOP.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurstWeapon : Weapon
{
    public float accuracy = 10;

    public float totalShots = 3;

    protected float FireRate => cooldown / (totalShots * 4);

    protected bool firing;
    protected BurstData data;

    protected struct BurstData
    {
        public BaseController sender;
        public float angle;
        public WeaponStats mods;
        public int shots;
        public float nextShot;
    }

    protected override void OnUpdate()
    {
        if (!firing) return;

        if (data.shots >= totalShots)
        {
            firing = false;
            data = default;
            return;
        }

        if (data.nextShot > 0)
        {
            data.nextShot -= Time.deltaTime;
            return;
        }

        SpawnProjectile(data.sender, data.angle, data.mods);

        data.shots++;
        data.nextShot = FireRate;
    }

    protected ProjectileHandler SpawnProjectile(BaseController sender, float angle, WeaponStats mods)
    {
        Vector2 hit = sender.Body.ShotVector(angle
                + Random.Range(-accuracy, accuracy));
        ProjectileHandler projectile = SpawnProjectile(sender, hit, mods, totalDamage);
        return projectile;
    }

    protected override ProjectileHandler FireProjectiles(BaseController sender,
        float angle, WeaponStats mods)
    {
        firing = true;
        data = new BurstData()
        {
            sender = sender,
            angle = angle,
            mods = mods,
            shots = 1,
        };

        return SpawnProjectile(sender, angle, mods);
    }
}
