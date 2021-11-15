using IgnitedBox.Entities;
using Scripts.OOP.Audio;
using Scripts.OOP.Stats;
using UnityEngine;

class FlameThrower : BurstWeapon
{
    const string desc = "Fires bursts of flames in short and inaccurate range.";

    protected override string Description => desc;

    public override float Range => (life * speed) / 5;

    public override void DefaultPreset()
    {
        cooldown = 1.2f;
        force = 12;
        totalDamage = 12;
        life = 0.5f;
        speed = 20;
        accuracy = 50;
        totalShots = 3;
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
