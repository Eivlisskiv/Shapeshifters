﻿using UnityEngine;

namespace IgnitedBox.Entities
{
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
