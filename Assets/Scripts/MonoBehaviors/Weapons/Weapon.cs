using Scripts.OOP.Audio;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.Stats;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    const string desc = "A single shot weapon";

    protected GameObject projectilPrefab;

    public float cooldown; //fire cooldown
    public float force; //fire cooldown

    public float totalDamage; //combined Damage of all projectiles
    public float life; //The life time of a projectile
    public float speed; //projection speed

    public virtual float Range => (speed * life) * 0.75f;

    protected virtual string Description => desc;

    protected string Name { get; private set; }

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

    protected virtual void OnStart()
    {
        projectilPrefab = LoadRessource<GameObject>("Projectile");
    }

    protected T LoadRessource<T>(string name) where T : Object
    {
        T item = Resources.Load<T>($"Weapons/{Name}/{name}");
        if(item == null) item = Resources.Load<T>($"Weapons/Default/{name}");
        return item;
    }

    public float Fire(BaseController sender, float angle)
    {
        WeaponStats stats = new WeaponStats();
        sender.perks.Activate<IWeaponFire>(1, perk => 
            perk.OnFire(angle, this, stats));

        FireProjectiles(sender, angle, stats);

        return force + stats.force;
    }

    protected virtual void FireProjectiles(BaseController sender,
        float angle, WeaponStats stats)
    {
        Vector2 hit = sender.body.ShotVector(angle);
        SpawnProjectile(sender, hit, stats, totalDamage);
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
        ProjectileHandler projectile, Collider2D collision)
    {
        if (projectile.IsSameSender(collision.gameObject)) return;

        BaseController controller = collision.gameObject
            .GetComponent<BaseController>();

        if (controller) controller.ProjectileHit(projectile);

        projectile.ToDestroy();
    }

    protected ProjectileHandler SpawnProjectile(
        BaseController sender, Vector2 direction,
        WeaponStats stats, float damage, float statsMod = 1)
    {
        GameObject projectile = Instantiate(projectilPrefab,
            GameModes.GetDebrisTransform(sender.team));

        projectile.name = $"{gameObject.name}_Projectile";

        projectile.transform.position = 
            (Vector2)transform.position + direction;

        ProjectileHandler handler = projectile
            .GetComponent<ProjectileHandler>();

        handler.onUpdate = OnProjectileUpdate;
        handler.onHit = OnProjectileHit;

        Collider2D collider = handler.body
            .GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(collider, sender.body.Collider);

        handler.SetStats(sender, damage * statsMod,
            (life + stats.life) / statsMod,
            ((speed + stats.speed) / statsMod) * direction.normalized,
            ((force + stats.force) / 4) * statsMod);

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
                volume = 0.1f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Retro Laser Gun (2)"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.1f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Retro Laser Gun (3)"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.1f,
                spacialBlend = 1,
            },

            new AudioEntity()
            {
                clip = LoadRessource<AudioClip>("Retro Laser Gun (5)"),
                replaySetting = AudioEntity.ReplaySetting.Restart,
                volume = 0.1f,
                spacialBlend = 1,
            },
        };
    }
}
