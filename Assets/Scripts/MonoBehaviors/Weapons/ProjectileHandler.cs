using Scripts.OOP.Utils;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public ProjectileBody body;
    public GameObject debrisPrefab;

    internal bool active = true;

    ParticleSystem particles;

    BaseController sender;

    public BaseController Sender
    {
        get => sender;
    }

    public float damage;
    public float force;

    internal float airtime;
    float lifeSpan;
    Vector2 velocity;

    bool dying = false;

    internal void SetStats(BaseController sender, 
        float damage, float range, Vector2 vector2, float force)
    {
        this.sender = sender;
        this.damage = damage;
        lifeSpan = range;
        velocity = vector2;
        this.force = force;
    }

    // Start is called before the first frame update
    void Start() 
    {
        particles = GetComponent<ParticleSystem>();
        transform.rotation = Quaternion.Euler(0,0,
            -90 + Vectors2.TrueAngle(Vector2.right, velocity));
    }

    // Update is called once per frame
    void Update()
    {
        if (dying) return;

        airtime += Time.deltaTime;
        if(lifeSpan < airtime)
        {
            ToDestroy();
            return;
        }

        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public bool IsSameSender(GameObject projectile)
    {
        ProjectileBody body = projectile.GetComponent<ProjectileBody>();
        return body && body.handler.sender == sender; 
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
