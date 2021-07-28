using UnityEngine;
using System;

public class ProjectileBody : MonoBehaviour
{
    [NonSerialized]
    public ProjectileHandler handler;

    public bool isOnStay = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOnStay) return;

        handler.OnCollide(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isOnStay) return;

        handler.OnCollide(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOnStay) return;

        handler.OnCollide(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isOnStay) return;

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
}
