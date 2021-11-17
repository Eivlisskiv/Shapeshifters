using UnityEngine;

namespace IgnitedBox.Entities
{
    public static class EntityUsage
    {
        public static bool TriggerEntity(this GameObject target, 
            out ISensitiveEntity entity)
        {
            if (target.TryGetComponent(out entity))
                return entity.Trigger();

            return false;
        }

        public static bool TriggerEntity<IProjectileType>(this GameObject target, 
            IProjectileType projectile, out ITargetEntity<IProjectileType> entity)
        {
            if (target.TryGetComponent(out entity))
                return entity.Trigger(projectile);

            return false;
        }
    }
}
