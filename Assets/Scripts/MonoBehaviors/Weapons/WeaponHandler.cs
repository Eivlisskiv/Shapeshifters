using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks.Weapon;
using Scripts.OOP.Stats;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Sprite pointProjectile;
    public Sprite dashProjectile;
    public GameObject projectilPrefab;

    public float angle; //The angle range of the attack
    public float size; //projectile size mult
    
    public float cooldown; //fire cooldown
    public float force; //fire cooldown

    public float totalDamage; //combined Damage of all projectiles
    public float life; //The life time of a projectile
    public float speed; //projection speed

    public float Range => speed * life;

    public float Fire(BaseController sender, float angle)
    {
        WeaponStats stats = new WeaponStats();
        sender.perks.Activate<IWeaponFire>(1, perk => perk.OnFire(angle, this, stats));

        (Vector2 barrel, bool)[] barrels = sender.body.GetPointsIn(angle, (this.angle + stats.angle) / 2);
        float damage = Mathf.FloorToInt((totalDamage + stats.totalDamage) / barrels.Length);
        for(int i = 0; i < barrels.Length; i++)
        {
            (Vector2 barrel, bool type) = barrels[i];
            SpawnProjectile(type, barrel, damage, stats, sender);
        }

        return force + stats.force;
    }

    private void SpawnProjectile(bool isBullet, Vector2 direction, float damage, WeaponStats stats, BaseController sender)
    {
        GameObject projectile = Instantiate(projectilPrefab, GameModes.GetDebrisTransform(sender.team));
        projectile.name = $"{gameObject.name}_Projectile";

        int m = (isBullet ? 1 : 2);

        projectile.transform.position = (Vector2)transform.position + direction;

        ProjectileHandler handler = projectile.GetComponent<ProjectileHandler>();
        float size = this.size + stats.size;
        handler.body.transform.localScale = new Vector3(size * m, size * m, 0);

        SpriteRenderer image = handler.body.GetComponent<SpriteRenderer>();
        image.sprite = isBullet ? pointProjectile : dashProjectile;

        PolygonCollider2D collider = handler.body.GetComponent<PolygonCollider2D>();

        Physics2D.IgnoreCollision(collider, sender.body.Collider);

        handler.SetStats(sender, damage * m, (life + stats.life) / m, 
            ((speed + stats.speed) / m) * direction.normalized, ((force + stats.force) / 4) * m);

        sender.perks.Activate<IProjectileFired>(1, perk => perk.OnFire(handler));
    }

}
