using UnityEngine;

namespace Assets.IgnitedBox.Entities
{
    public abstract class HealthEntity<TProjectile> : MonoBehaviour
    {
        public static bool HasHeathEntity(GameObject gameObject, TProjectile projectile)
        {
            HealthEntity<TProjectile> entity = gameObject.GetComponent<HealthEntity<TProjectile>>();

            if (!entity) return false;

            entity.ProjectileHit(projectile);

            return true;
        }

        public abstract float Health { get; }

        public abstract float MaxHealth { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod">The amount of health to be changed. Positive for healing, negative for damage.</param>
        /// <returns>If the entity should be considered dead</returns>
        public abstract bool ModifyHealth(float mod);

        public abstract void ProjectileHit(TProjectile projectile);
    }
}
