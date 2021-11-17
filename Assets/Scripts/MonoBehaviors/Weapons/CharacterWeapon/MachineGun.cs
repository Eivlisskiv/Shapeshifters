using Scripts.OOP.Audio;
using Scripts.OOP.Stats;
using UnityEngine;

public class MachineGun : Weapon
{
    const string desc = "Fires inaccurate bullets";

    public float accuracy;

    protected override string Description => desc;

    public override float Range => (speed * 5) / accuracy;

    public override void DefaultPreset()
    {
        cooldown = 0.15f;
        force = 1.8f;
        totalDamage = 3;
        life = 5;
        speed = 50;
        accuracy = 20;
    }

    protected override ProjectileHandler FireProjectiles(BaseController sender, 
        float angle, WeaponStats stats)
    {
        Vector2 hit = sender.Body.ShotVector(angle
            + Random.Range(-accuracy, accuracy));

        return SpawnProjectile(sender, hit, stats, totalDamage);
    }

    protected override void OnProjectileUpdate(ProjectileHandler projectile)
    {
        projectile.transform.position += (Vector3)projectile.Velocity
            * Time.deltaTime;
    }

    protected override AudioEntity[] GetFireClips()
    {
        return new AudioEntity[]
        {
            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Gun 1"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.2f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Gun 2"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.2f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Gun 3"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.2f,
                spacialBlend = 1,
            },

        };
    }
}
