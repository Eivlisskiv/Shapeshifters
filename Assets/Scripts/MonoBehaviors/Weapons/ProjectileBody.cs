using UnityEngine;
using System;
using IgnitedBox.Entities;
using Scripts.Explosion;

public class ProjectileBody : MonoBehaviour,
    ITargetEntity<ProjectileHandler>, ITargetEntity<ExplosionHandler.Effect>
{
    [NonSerialized]
    public ProjectileHandler handler;

    public bool isOnStay = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(isOnStay) return;

        if (collider.isTrigger) return;

        handler.OnCollide(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!isOnStay) return;

        if (collider.isTrigger) return;

        handler.OnCollide(collider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOnStay) return;

        if (collision.collider.isTrigger) return;

        handler.OnCollide(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isOnStay) return;

        if (collision.collider.isTrigger) return;

        handler.OnCollide(collision.collider);
    }

    private void OnDestroy()
    {
        DestroyComponent<SpriteRenderer>();
        DestroyComponent<Rigidbody2D>();
        DestroyComponent<Collider2D>();
    }

    private void DestroyComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component) Destroy(component);
    }

    public bool Trigger(ProjectileHandler projectile)
    {
        handler.OnCollide(null);
        return false;
    }

    public bool Trigger(ExplosionHandler.Effect projectile)
    {
        handler.OnCollide(null);
        return false;
    }
}
