using Scripts.OOP.Stats;
using UnityEngine;

public class LaserShot : Weapon
{
    const string desc = "A multi shot weapon with a mix or long range " +
        "and high damage projectiles.";

    public float angle; //The angle range of the attack
    public float size; //projectile size mult

    private Sprite pointProjectile;
    private Sprite dashProjectile;

    protected override string Description => desc;

    public override void DefaultPreset()
    {
        cooldown = 0.8f;
        force = 10;
        totalDamage = 30;
        life = 1.5f;
        speed = 10;
        angle = 45;
        size = 1;
    }

    protected override void OnStart()
    {
        base.OnStart();

        pointProjectile = LoadResource<Sprite>("Point");
        dashProjectile = LoadResource<Sprite>("Dash");
    }

    protected override ProjectileHandler FireProjectiles(BaseController sender,
        float angle, WeaponStats stats)
    {
        (Vector2 barrel, bool)[] barrels = sender.Body.GetPointsIn(angle,
            (this.angle + stats.angle) / 2);

        if (barrels.Length == 0)
            return base.FireProjectiles(sender, angle, stats);

        float damage = Mathf.FloorToInt((totalDamage + stats.totalDamage)
            / barrels.Length);

        ProjectileHandler projectile = null;
        for (int i = 0; i < barrels.Length; i++)
        {
            (Vector2 barrel, bool type) = barrels[i];
            projectile = SpawnProjectile(type, barrel, damage, stats, sender);
        }

        return projectile;
    }

    protected ProjectileHandler SpawnProjectile(bool isBullet, Vector2 direction,
        float damage, WeaponStats stats, BaseController sender)
    {
        int m = (isBullet ? 1 : 2);

        ProjectileHandler handler = SpawnProjectile(sender, direction,
            stats, damage, m);

        float size = this.size + stats.size;

        handler.body.transform.localScale =
            new Vector3(size * m, size * m, 0);

        SpriteRenderer image = handler.body.GetComponent<SpriteRenderer>();
        image.sprite = isBullet ? pointProjectile : dashProjectile;

        return handler;
    }
}
