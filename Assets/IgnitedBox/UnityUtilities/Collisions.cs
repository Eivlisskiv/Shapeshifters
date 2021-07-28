using IgnitedBox.UnityUtilities;
using System;
using UnityEngine;

namespace Assets.IgnitedBox.UnityUtilities
{
    public static class Collisions
    {
        public static Collision2DHandler AddCollisionHandler<TCollider>(this GameObject go,
            bool isOnFrame, Action<Collider2D> onCollide) where TCollider : Collider2D
        {
            return go.AddComponent<Collision2DHandler>(handler =>
                handler.Set<TCollider>(isOnFrame, onCollide));
        }

        public static Collision2DHandler AddCollisionHandler<TCollider>(this GameObject go,
            bool isOnFrame, Action<Collision2D> onCollide) where TCollider : Collider2D
        {
            return go.AddComponent<Collision2DHandler>(handler =>
                handler.Set<TCollider>(isOnFrame, onCollide));
        }
    }
}
