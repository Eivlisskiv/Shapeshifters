using Scripts.OOP.Audio;
using Scripts.OOP.Stats;
using Scripts.OOP.Utils;
using UnityEngine;

public class MachineGun : Weapon
{
    const string desc = "Fires inaccurate bullets";

    public float accuracy;

    protected override string Description => desc;

    public override float Range => (speed * 5) / accuracy;

    protected override void FireProjectiles(BaseController sender, 
        float angle, WeaponStats stats)
    {
        Vector2 hit = sender.body.ShotVector(angle
            + Random.Range(-accuracy, accuracy));

        SpawnProjectile(sender, hit, stats, totalDamage);
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
