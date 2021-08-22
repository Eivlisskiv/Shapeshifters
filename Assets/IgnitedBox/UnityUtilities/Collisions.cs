using IgnitedBox.UnityUtilities;
using System;
using UnityEngine;

namespace Assets.IgnitedBox.UnityUtilities
{
    public static class Collisions
    {
        public static Collision2DHandler AddCollisionHandler<TCollider>(this GameObject go,
            float? isOnFrame, Action<Collider2D> onCollide) where TCollider : Collider2D
        {
            return isOnFrame.HasValue ?
                go.AddComponent<Collision2DHandler>(handler =>
                handler.Set<TCollider>(isOnFrame.Value, onCollide))
                :
                go.AddComponent<Collision2DHandler>(handler =>
                handler.Set<TCollider>(onCollide));
        }

        public static Collision2DHandler AddCollisionHandler<TCollider>(this GameObject go,
            float? isOnFrame, Action<Collision2D> onCollide) where TCollider : Collider2D
        {
            return isOnFrame.HasValue ?
                go.AddComponent<Collision2DHandler>(handler =>
                handler.Set<TCollider>(isOnFrame.Value, onCollide)) 
                :
                go.AddComponent<Collision2DHandler>(handler =>
                handler.Set<TCollider>(onCollide));
        }
    }
}
