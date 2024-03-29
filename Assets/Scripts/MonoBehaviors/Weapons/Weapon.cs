﻿using IgnitedBox.Entities;
using Scripts.OOP.Audio;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static System.Type[] Types = GetWeaponTypes();

    private static System.Type[] GetWeaponTypes()
    {
        System.Type wep = typeof(Weapon);
        return wep.Assembly.GetTypes().Where(t => 
            !t.IsAbstract && !t.IsInterface &&
            t.IsSubclassOf(wep)).ToArray();
    }

    private static readonly Dictionary<System.Type, Sprite> icons
        = new Dictionary<System.Type, Sprite>();

    public static Sprite GetIcon(System.Type weapon)
    {
        if (icons.TryGetValue(weapon, out Sprite icon)) return icon;

        icon = Resources.Load<Sprite>($"Weapons/{weapon.Name}/Icon");
        if(!icon) icon = Resources.Load<Sprite>($"Weapons/Default/Icon");
        if (!icon) return null;

        icons.Add(weapon, icon);

        return icon;

    }

    const string desc = "A single shot weapon";

    protected GameObject projectilPrefab;

    public float cooldown; //fire cooldown
    public float force; //fire cooldown

    public float totalDamage; //combined Damage of all projectiles
    public float life; //The life time of a projectile
    public float speed; //projection speed

    public virtual float Range => (speed * life) * 0.72f;

    protected virtual string Description => desc;

    public Sprite Icon => GetIcon(GetType());

    protected string Name { get; private set; }

    public virtual void DefaultPreset()
    {
        cooldown = 0.8f;
        force = 10;
        totalDamage = 30;
        life = 1.5f;
        speed = 15;
    }

    private void Start()
    {
        Name = GetType().Name;

        BaseController controller = GetComponent<BaseController>();

        controller.Sounds.UpdateClips(new RandomClips()
        {
            name = "Fire",
            clips = GetFireClips()
        });

        OnStart();
    }

    private void Update() => OnUpdate();

    protected virtual void OnStart()
    {
        projectilPrefab = LoadRessource<GameObject>("Projectile");
    }

    protected virtual void OnUpdate() { }

    protected T LoadRessource<T>(string name) where T : Object
    {
        T item = Resources.Load<T>($"Weapons/{Name}/{name}");
        if(item == null) item = Resources.Load<T>($"Weapons/Default/{name}");
        return item;
    }

    public (float, ProjectileHandler) Fire(BaseController sender, float angle)
    {
        WeaponStats stats = new WeaponStats();
        sender.perks.Activate<IWeaponFire>(1, perk => 
            perk.OnFire(angle, this, stats));

        var projectile = FireProjectiles(sender, angle, stats);

        return (force + stats.force, projectile);
    }

    protected virtual ProjectileHandler FireProjectiles(BaseController sender,
        float angle, WeaponStats stats)
    {
        Vector2 hit = sender.Body.ShotVector(angle);
        return SpawnProjectile(sender, hit, stats, totalDamage);
    }

    protected virtual void OnProjectileUpdate(ProjectileHandler projectile)
    {
        if (projectile.LifeSpan < projectile.Airtime)
        {
            projectile.ToDestroy();
            return;
        }

        projectile.transform.position += (Vector3)projectile.Velocity 
            * Time.deltaTime;
    }

    protected virtual void OnProjectileHit(
        ProjectileHandler projectile, Collider2D collider)
    {
        if (collider)
        {
            if (projectile.IsSameSender(collider.gameObject)) return;

            collider.gameObject.TriggerEntity(projectile, out _);
        }

        projectile.ToDestroy();
    }

    protected ProjectileHandler SpawnProjectile(
        BaseController sender, Vector2 direction,
        WeaponStats mods, float damage, float statsMod = 1)
    {
        GameObject projectile = Instantiate(projectilPrefab,
            GameModes.GetDebrisTransform(sender.Team));

        projectile.name = $"{gameObject.name}_Projectile";

        projectile.transform.position = 
            (Vector2)transform.position + direction;

        ProjectileHandler handler = projectile
            .GetComponent<ProjectileHandler>();

        handler.OnUpdate = OnProjectileUpdate;
        handler.OnHit = OnProjectileHit;

        Collider2D collider = handler.body
            .GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(collider, sender.Body.Collider);

        handler.SetStats(sender, damage * statsMod,
            (life + mods.life) / statsMod,
            ((speed + mods.speed) / statsMod) * direction.normalized,
            ((force + mods.force) / 4) * statsMod);

        sender.perks.Activate<IProjectileFired>(1, perk =>
            perk.OnFire(handler));

        return handler;
    }

    protected virtual AudioEntity[] GetFireClips()
    {
        return new AudioEntity[]
        {
            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Retro Laser Gun (1)"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Retro Laser Gun (2)"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Retro Laser Gun (3)"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Retro Laser Gun (5)"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                spacialBlend = 1,
            },
        };
    }
}
