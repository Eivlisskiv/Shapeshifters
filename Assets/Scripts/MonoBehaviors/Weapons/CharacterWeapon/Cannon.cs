using IgnitedBox.Entities;
using Scripts.OOP.Audio;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.Stats;
using System;
using UnityEngine;

public class Cannon : Weapon
{
    const string desc = "Fires a cannon ball dealing impact damage." +
        "Impact damage ignores shields.";

    protected override string Description => desc;

    public override float Range => (speed * life) * 0.5f;

    public override void DefaultPreset()
    {
        cooldown = 1.2f;
        force = 12;
        totalDamage = 22;
        life = 5;
        speed = 15;
    }

    protected override ProjectileHandler FireProjectiles(BaseController sender, float angle, WeaponStats stats)
    {
        Vector2 hit = sender.Body.ShotVector(angle);
        var projectile = SpawnProjectile(sender, hit, stats, totalDamage);

        projectile.Rigidbody.velocity += 
            projectile.Velocity * projectile.force;

        return projectile;
    }

    protected override void OnProjectileHit
        (ProjectileHandler projectile, Collider2D collision)
    {
        if (projectile.IsSameSender(collision.gameObject)) return;

        ITargetEntity<ProjectileHandler> entity = collision.gameObject
            .GetComponent<ITargetEntity<ProjectileHandler>>();

        if (entity == null) return;

        if (entity is BaseController victim)
        {
            TargetController(projectile, victim);
            return;
        }

        entity.Trigger(projectile);
    }

    private void TargetController(ProjectileHandler projectile, BaseController victim)
    {
        if (!victim || victim.IsTeammate(projectile.Sender)) return;

        projectile.Sender.perks.Activate<IProjectileHitTarget>(1,
            perk => perk.OnHit(projectile, victim));

        float damage = Math.Min(projectile.damage,
            (projectile.Rigidbody.velocity.magnitude
                + victim.Body.Body.velocity.magnitude));

        victim.ProcessDamage(damage, projectile.Sender,
            projectile.transform.position);

        float push = (damage * projectile.force) / 8;

        victim.ApplyCollisionForce(projectile.transform.position, push / 2, push);
    }

    protected override void OnProjectileUpdate(ProjectileHandler projectile)
    {
        if (projectile.LifeSpan < projectile.Airtime)
        {
            projectile.ToDestroy();
            return;
        }

        float s = 1 - (projectile.Airtime / projectile.LifeSpan);

        projectile.transform.localScale =
            new Vector3(s, s, 1);
    }

    protected override AudioEntity[] GetFireClips()
    {
        return new AudioEntity[]
        {
            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Cannon Shot"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 1f,
                spacialBlend = 1,
            },
        };
    }
}

