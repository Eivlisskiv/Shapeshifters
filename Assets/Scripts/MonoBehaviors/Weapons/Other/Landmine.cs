using UnityEngine;

public class Landmine : MonoBehaviour
{
    Collider2D bodyCollider;
    BaseController owner;
    float damage;
    float force;

    bool active;

    public void Activate(float damage, float force, BaseController owner)
    {
        this.damage = damage;
        this.force = force;
        this.owner = owner;
        active = true;

        bodyCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bodyCollider, owner.body.Collider);

        Destroy(gameObject, 300f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!active) return;

        BaseController victim = collision.gameObject.GetComponent<BaseController>();
        if (!victim || owner.IsTeammate(victim)) return;

        victim.TakeDamage(damage, owner, transform.position);
        victim.ApplyCollisionForce(transform.position, force, force);

        active = false;

        GetComponent<SpriteRenderer>().enabled = false;
        bodyCollider.enabled = false;

        ParticleSystem particles = GetComponent<ParticleSystem>();
        particles.Play();

        Destroy(gameObject, 1f);
    }
}
