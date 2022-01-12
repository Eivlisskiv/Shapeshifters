using System.Linq;
using UnityEngine;

namespace Assets.IgnitedBox.UnityUtilities
{
    public static class Raycast
    {
        public static bool TryRaycast2D(Vector2 startPosition, Vector2 vectorRay, 
            out RaycastHit2D raycast, params Collider2D[] ignores)
        {
            var rays = Physics2D.RaycastAll(startPosition, vectorRay, vectorRay.magnitude);

            return GetFirstUnignoredRaycast(out raycast, ignores, rays);
        }

        public static bool GetFirstUnignoredRaycast(out RaycastHit2D raycast, Collider2D[] ignores, RaycastHit2D[] rays)
        {
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

        public static bool TryRaycast2D(Vector2 startPosition, Vector2 vectorRay,
            int layerNumber, out RaycastHit2D raycast)
        {
            return (raycast = Physics2D.Raycast(startPosition, vectorRay, vectorRay.magnitude, 1 << layerNumber));
        }

        public static bool TryRaycast2D(Vector2 startPosition, Vector2 vectorRay,
            out RaycastHit2D raycast)
        {
            return (raycast = Physics2D.Raycast(startPosition, vectorRay, vectorRay.magnitude));
        }

        public static bool CanSee(GameObject from, GameObject to, int blockLayerNumber)
        {
            Vector3 fromPos = from.transform.position;
            Vector3 dir = to.transform.position - fromPos;

            return !Physics2D.Raycast(fromPos, dir, dir.magnitude, 1 << blockLayerNumber);
        }

        public static bool CanSee(Component from, Component to, int blockLayerNumber)
        {
            Vector3 fromPos = from.transform.position;
            Vector3 dir = to.transform.position - fromPos;

            return !Physics2D.Raycast(fromPos, dir, dir.magnitude, 1 << blockLayerNumber);
        }
    }
}
