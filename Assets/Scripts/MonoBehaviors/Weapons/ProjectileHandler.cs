using Scripts.OOP.Utils;
using System;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public ProjectileBody body;

    public Rigidbody2D Rigidbody 
    {
        get
        {
            if (!_rigidbody) _rigidbody = GetComponent<Rigidbody2D>();
            return _rigidbody;
        } 
    }

    private Rigidbody2D _rigidbody;

    internal bool active = true;

    ParticleSystem particles;

    public BaseController Sender { get; private set; }

    public Action<ProjectileHandler, Collider2D> OnHit { private get; set; }
    public Action<ProjectileHandler> OnUpdate { private get; set; }

    public float damage;
    public float force;

    public float Airtime { get; private set; }

    public float LifeSpan { get; private set; }

    public Vector2 Velocity { get; private set; }

    bool dying = false;

    internal void SetStats(BaseController sender, 
        float damage, float range, Vector2 vector2, float force)
    {
        Sender = sender;
        this.damage = damage;
        LifeSpan = range;
        Velocity = vector2;
        this.force = force;
    }

    // Start is called before the first frame update
    void Start() 
    {
        particles = GetComponent<ParticleSystem>();
        transform.rotation = Quaternion.Euler(0,0,
            -90 + Vectors2.TrueAngle(Vector2.right, Velocity));

        body.handler = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (dying) return;

        Airtime += Time.deltaTime;

        OnUpdate?.Invoke(this);
    }

    public bool IsSameSender(GameObject projectile)
    {
        ProjectileBody body = projectile.GetComponent<ProjectileBody>();
        return body && body.handler.Sender == Sender;
    }

    public void OnCollide(Collider2D collision)
    {
        if (dying) return;

        OnHit?.Invoke(this, collision);
    }

    public void ToDestroy()
    {
        if (!dying)
        {
            dying = true;
            particles.Stop();
            Destroy(body.gameObject);
            Destroy(gameObject, 1f);
        }
    }
}
