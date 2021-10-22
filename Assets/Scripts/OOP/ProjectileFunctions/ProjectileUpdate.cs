using System;
using UnityEngine;

namespace Scripts.OOP.ProjectileFunctions
{
    public static class ProjectileUpdate
    {
        public static void Default(ProjectileHandler projectile)
        {
            if (ProjectileExpired(projectile)) return;
            MoveWithVelocity(projectile);
        }

        public static void SeekTarget(ProjectileHandler projectile, Transform target)
        {
            Vector2 toTarget = target.position - projectile.transform.position;

            Rotate(projectile, Vector2.Angle(projectile.transform.up, toTarget)
                * projectile.Velocity.magnitude * Time.deltaTime);

            projectile.Velocity = projectile.transform.up;

            MoveWithVelocity(projectile);
        }

        public static Action<ProjectileHandler> Missile(Transform target)
        {
            return (projectile) =>
            {
                if (projectile.Airtime < 0.5f) return;

                if (!projectile.active)
                {
                    projectile.active = true;
                    if (projectile.TryGetComponent(
                        out ParticleSystem particles))
                        particles.Play();
                }

                SeekTarget(projectile, target);
            };
        }

        public static void Rotate(ProjectileHandler projectile, float rotation)
            => projectile.transform.Rotate(new Vector3(0, 0, 1), rotation);

        public static void MoveWithVelocity(ProjectileHandler projectile)
            => projectile.transform.position += (Vector3)projectile.Velocity * Time.deltaTime;

        public static bool ProjectileExpired(ProjectileHandler projectile)
        {
            if (projectile.LifeSpan < projectile.Airtime)
            {
                projectile.ToDestroy();
                return true;
            }

            return false;
        }
    }
}
