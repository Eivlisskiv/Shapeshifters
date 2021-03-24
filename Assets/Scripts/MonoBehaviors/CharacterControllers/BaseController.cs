using Scripts.OOP.Character.Stats;
using Scripts.OOP.GameModes;
using Scripts.OOP.Perks;
using Scripts.OOP.Perks.Character.Triggers;
using Scripts.OOP.Perks.Weapon;
using Scripts.OOP.TileMaps;
using Scripts.OOP.Utils;
using System;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public WeaponHandler weapon;
    public BodyPhysicsHandler body;

    internal int team;

    private SoundHandler sounds;

    float cooldown = 0;
    public bool FireReady => cooldown <= 0;
    public Stats stats;

    internal PerksHandler perks;

    protected int level;
    public int Level
    { get => level; }

    protected float xp;
    public int XPRequired => Mathf.FloorToInt((level + 1) * (float)Math.Pow(10, 2));

    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            if (body) body.Color = value;
        }
    }

    float? dying = null;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<BodyPhysicsHandler>();
        sounds = GetComponent<SoundHandler>();
        body.Color = _color;
        weapon = GetComponent<WeaponHandler>();
        OnStart();
        if (stats == null) stats = new Stats(50 + (level / 5));
        if (perks == null) perks = new PerksHandler();
    }

    public abstract void OnStart();

    // Update is called once per frame
    void Update()
    {
        if (CheckDying()) return;

        OnUpdate();

        perks.Activate<IControllerUpdate>(Time.deltaTime, 
            perk => perk.OnControllerUpdate(this, Time.deltaTime));

        if (!FireReady) cooldown = Math.Max(cooldown - Time.deltaTime, 0);
        else if (IsFiring(out float angle)) Fire(angle);

        float hpp = (stats.HPP / 2) + 0.5f;
        if(hpp != transform.localScale.x)
            transform.localScale = Math.Abs(transform.localScale.x - hpp) < 0.01 ?
                new Vector3(hpp, hpp, 1) : Vector3.Lerp(transform.localScale,
                new Vector3(hpp, hpp, 1), Time.deltaTime);
    }

    private bool CheckDying()
    {
        if (dying.HasValue)
        {
            dying = Math.Max(0, dying.Value - Time.deltaTime);
            if (dying > 0)
            {
                body.Body.velocity = Vector2.zero;
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 1), Time.deltaTime);
            }
            else
            {
                transform.localScale = Vector3.zero;
                var debris = Instantiate(body.deathPrefab);
                debris.transform.position = transform.position;
                ParticleSystem.MainModule settings = debris.GetComponent<ParticleSystem>().main;
                settings.startColor = new ParticleSystem.MinMaxGradient(Color);
                Destroy(debris, 2f);
                OnDeathEnded();
                Destroy(gameObject);
            }

            return true;
        }

        return false;
    }

    public abstract void OnUpdate();
    public abstract bool IsFiring(out float angle);

    private void Fire(float angle)
    {
        sounds.PlayRandom("Fire");
        cooldown = weapon.cooldown;
        float strength = weapon.Fire(this, angle);
        body.AddForce(angle, strength, strength);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        perks.Activate<ICollide>(1, perk => perk.OnCollide(this, collision));

        sounds.PlayRandom("Collide");
        //Apply collision force
        for (int i = 0; i < collision.contactCount; i++)
            ApplyCollisionForce(collision.contacts[i], 0);
    }

    public void ProjectileHit(ProjectileHandler projectile)
    {
        perks.Activate<IProjectileTaken>(1,
            perk => perk.OnHit(this, projectile));

        if (!projectile || !projectile.active) return;

        float push = projectile.force;
        if (!IsTeammate(projectile.Sender))
        {
            projectile.Sender.perks.Activate<IProjectileHitTarget>(1,
                perk => perk.OnHit(projectile, this));

            TakeDamage(projectile.damage, projectile.Sender,
                projectile.transform.position);
        }

        ApplyCollisionForce(projectile.transform.position, 0, push);
    }

    public bool IsTeammate(BaseController other)
        => GetType().Equals(other.GetType());

    private void ApplyCollisionForce(ContactPoint2D contact, float push)
        => ApplyCollisionForce(contact.point,
            contact.relativeVelocity.magnitude, push);
    public void ApplyCollisionForce(Vector2 hit, float magnitude, float push)
    {
        Vector2 pos = transform.position;
        int y = pos.y > hit.y ? -1 : 1;
        float angle = Vector2.Angle(Vector2.left, pos - hit) * y;
        if (push > 2) body.AddForce(angle, magnitude, push);
    }

    public void TakeDamage(float damage, BaseController attacker, Vector2? direction)
    {
        if (direction.HasValue) SpawnHitParticles(direction.Value);
        if (HitHealth(-damage))
        {
            if(attacker) attacker.OnKill(this);
            OnDeath();
        }
    }

    private void SpawnHitParticles(Vector2 position)
    {
        sounds.PlayRandom("Hit");
        if (body && body.hitPrefab)
        {
            var debris = Instantiate(body.hitPrefab, AGameMode.GetDebrisTransform(team));
            debris.transform.position = position;
            debris.transform.rotation = Quaternion.Euler(0, 0, 
                Vectors2.TrueAngle(transform.position, position));
            ParticleSystem.MainModule settings = debris.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Color);
            Destroy(debris, 0.5f);
        }
    }

    public bool HitHealth(float value)
    {
        stats.health = Math.Min(stats.MaxHealth, stats.health + value);
        OnHealthChange();
        if (!dying.HasValue && stats.health <= 0 && OnDeath())
        {
            dying = 1.5f;
            body.Body.velocity = Vector2.zero;
            body.Body.angularVelocity = 720;
            body.Collider.enabled = false;
            return true;
        }
        return false;
    }

    public abstract void OnHealthChange();

    public abstract bool OnDeath();
    public abstract void OnDeathEnded();

    public void OnKill(BaseController target)
    {
        AddXP(target.xp + (target.XPRequired / (level + 2) ));
        if(target is AIController && target.perks.Count > 0)
            perks.Add(target.perks.RandomDrop(), this is PlayerController player ? player.UI : null);
    }

    private void AddXP(float amount)
    {
        xp += amount;
        bool up = false;
        while(xp >= XPRequired)
        {
            xp -= XPRequired;
            up = true;
            level++;
            HitHealth(stats.MaxHealth - stats.health);
        }

        if (up)
        {
            AGameMode.Run<IControllerLevelUp>(mode =>
                mode.ControllerLevelUp(this));
        }
        OnXPChange(up);
    }

    public abstract void OnXPChange(bool isUp);
}
