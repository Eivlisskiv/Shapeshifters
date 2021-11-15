using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability.ProjectileLaunchers
{
    public abstract class ProjectileLauncher<ProjectileType> : GameObjectSpawner<ProjectileType>
        where ProjectileType : Component
    {
        protected ProjectileLauncher(string path) : base("Projectiles/" + path)
        { }

        protected virtual ProjectileType Fire(BaseController self, Vector2 velocity)
        {
            if (!base.TryInstantiate(out _, out ProjectileType projectile)) return null;

            projectile.transform.position = self.transform.position
                + (Vector3)(velocity * self.Body.Radius * 2);
            return projectile;
        }

        protected abstract void OnProjectileUpdate(ProjectileType projectile);
        protected abstract void OnProjectileHit(ProjectileType projectile, Collider2D collider);
    }
}
