using System.Linq;
using UnityEngine;

namespace Assets.IgnitedBox.UnityUtilities
{
    public static class Raycast
    {
        public static bool TryRaycast2d(Vector2 startPosition, Vector2 vectorRay, 
            out RaycastHit2D raycast, params Collider2D[] ignores)
        {
            var rays = Physics2D.RaycastAll(startPosition, vectorRay, vectorRay.magnitude);

            for (int i = 0; i < rays.Length; i++)
            {
                RaycastHit2D ray = rays[i];

                if (ignores.Contains(ray.collider)) continue;

                raycast = ray;
                return true;
            }

            raycast = default;
            return false;
        }
    }
}
