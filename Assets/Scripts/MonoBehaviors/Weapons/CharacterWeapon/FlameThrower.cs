using Assets.IgnitedBox.Entities;
using Scripts.OOP.Audio;
using Scripts.OOP.Stats;
using UnityEngine;

class FlameThrower : Weapon
{
    const string desc = "Fires flames in short range.";

    public float accuracy = 50;

    protected override string Description => desc;

    public override float Range => (life * speed) / 5;

    public override void DefaultPreset()
    {
        cooldown = 0.1f;
        force = 1;
        totalDamage = 1;
        life = 0.3f;
        speed = 10;
        accuracy = 50;
    }

    protected override void FireProjectiles(BaseController sender,
        float angle, WeaponStats stats)
    {
        Vector2 hit = sender.body.ShotVector(angle
            + Random.Range(-accuracy, accuracy));

        SpawnProjectile(sender, hit, stats, totalDamage);
    }
    protected override void OnProjectileHit(ProjectileHandler projectile, Collider2D collision)
    {
        if (projectile.IsSameSender(collision.gameObject)) return;

        HealthEntity<ProjectileHandler>.HasHeathEntity
            (collision.gameObject, projectile);
    }

    protected override AudioEntity[] GetFireClips()
    {
        return new AudioEntity[]
        {
            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Fire 1"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.9f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Fire 2"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.9f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Fire 3"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.9f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Fire 4"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.9f,
                spacialBlend = 1,
            },
        };
    }
}
