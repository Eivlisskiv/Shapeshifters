using IgnitedBox.Entities;
using UnityEngine;

namespace Scripts.OOP.ProjectileFunctions
{
    public static class ProjectileHit
    {
        public static void Default(ProjectileHandler projectile, Collider2D collider)
        {
            if (projectile.IsSameSender(collider.gameObject)) return;

            collider.gameObject.TriggerEntity(projectile, out _);

            projectile.ToDestroy();
        }
    }
}
