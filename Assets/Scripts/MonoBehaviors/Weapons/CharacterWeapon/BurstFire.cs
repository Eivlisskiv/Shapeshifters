using Scripts.OOP.Stats;
using System;
using UnityEngine;

public class BurstFire : Weapon
{
    public float accuracy = 10;

    public float totalShots = 3;

    private bool firing;
    private BurstData data;

    private float FireRate => cooldown / (totalShots * 2);

    private struct BurstData
    {
        public BaseController sender;
        public float angle;
        public WeaponStats stats;
        public int shots;
        public float nextShot;
    }

    public override void DefaultPreset()
    {
        cooldown = 0.8f;
        force = 10;
        totalDamage = 15;
        life = 3;
        speed = 25;

        accuracy = 10;
        totalShots = 3;
    }

    protected override void OnUpdate()
    {
        if (!firing) return;

        if(data.shots >= totalShots)
        {
            firing = false;
            data = default;
            return;
        }

        if(data.nextShot > 0)
        {
            data.nextShot -= Time.deltaTime;
            return;
        }

        Vector2 hit = data.sender.body.ShotVector(data.angle
                + UnityEngine.Random.Range(-accuracy, accuracy));
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
