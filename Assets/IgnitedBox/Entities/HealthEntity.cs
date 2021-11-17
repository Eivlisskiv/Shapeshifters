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

    public interface ISensitiveEntity
    {
        /// <summary>
        /// The entity was triggered
        /// </summary>
        bool Trigger();
    }

    public interface ITargetEntity<IProjectileType>
    {
        bool Trigger(IProjectileType projectile);
    }

    public abstract class HealthEntity : MonoBehaviour
    {
        public abstract float Health { get; }

        public abstract float MaxHealth { get; }

        /// <summary>
        /// Modify the health of the Health Entity.
        /// </summary>
        /// <param name="mod">The amount of health to be changed. Positive for healing, negative for damage.</param>
        /// <returns>If the entity should be considered dead</returns>
        public abstract bool ModifyHealth(float mod);

        public abstract void ApplyCollisionForce(Vector2 hit, float magnitude, float push);
    }
}
