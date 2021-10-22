using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability.ProjectileLaunchers
{
    public abstract class ProjectileLauncher : GameObjectSpawner<ProjectileHandler>
    {
        protected ProjectileLauncher(string path) : base("Projectiles/" + path)
        { }

        protected virtual ProjectileHandler Fire(BaseController self, float damage, float range, Vector2 velocity, float force)
        {
            if (!base.TryInstantiate(out _, out ProjectileHandler projectile)) return null;

            projectile.transform.position = self.transform.position
                + (Vector3)(velocity * self.Body.Radius * 2);
            projectile.SetStats(self, damage, range, velocity, force);
            projectile.OnUpdate = OnProjectileUpdate;
            projectile.OnHit = OnProjectileHit;

            return projectile;
        }

        protected abstract void OnProjectileUpdate(ProjectileHandler projectile);
        protected abstract void OnProjectileHit(ProjectileHandler projectile, Collider2D collider);
    }
}
