using Scripts.OOP.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurstWeapon : Weapon
{
    public float accuracy = 10;

    public float totalShots = 3;

    protected float FireRate => cooldown / (totalShots * 2);

    protected bool firing;
    protected BurstData data;

    protected struct BurstData
    {
        public BaseController sender;
        public float angle;
        public WeaponStats stats;
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

        Vector2 hit = data.sender.body.ShotVector(data.angle
                + Random.Range(-accuracy, accuracy));
        SpawnProjectile(data.sender, hit, data.stats, totalDamage);

        data.shots++;
        data.nextShot = FireRate;
    }

    protected override void FireProjectiles(BaseController sender,
        float angle, WeaponStats stats)
    {
        firing = true;
        data = new BurstData()
        {
            sender = sender,
            angle = angle,
            stats = stats,
            shots = 0,
        };
    }
}
