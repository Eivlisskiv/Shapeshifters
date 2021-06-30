using Scripts.OOP.Character.Stats;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks;
using Scripts.OOP.Perks.Character.Triggers;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.Utils;
using System;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public Weapon Weapon
        => weapon ? weapon : (weapon = GetComponent<Weapon>());

    private Weapon weapon;

    [NonSerialized]
    public BodyPhysicsHandler body;

    internal int team;

    public SoundHandler Sounds 
    { 
        get
        {
            if (!_sounds)  _sounds = GetComponent<SoundHandler>();
            return _sounds;
        }
    }
    private SoundHandler _sounds;

    float cooldown = 0;
    public bool FireReady => cooldown <= 0;
    public Stats stats;

    internal PerksHandler perks;

    protected int level;
    public int Level
    { get => level; }

    protected float xp;
    public int XPRequired => Mathf.FloorToInt((level + 1) * (float)Math.Pow(10, 2));

    private readonly Color[] _colors = new Color[2];
    public void SetColor(int i, Color value)
    {
        _colors[i] = value;
        if (body) body.SetColor(i, value);
    }

    bool controllerEnabled = true;
    float? dying = null;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<BodyPhysicsHandler>();
        body.SetColor(0, _colors[0]);
        body.SetColor(1, _colors[1]);

        OnStart();

        if (stats == null) stats = new Stats(30 + (level / 5));
        if (perks == null) perks = new PerksHandler();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckDying()) return;

        if (!controllerEnabled) return;

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

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract bool IsFiring(out float angle);

    public void DisableController(bool value)
        => controllerEnabled = !value;

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
                settings.startColor = new ParticleSystem.MinMaxGradient(_colors[0]);
                Destroy(debris, 2f);
                OnDeathEnded();
                Destroy(gameObject);
            }

            return true;
        }

        return false;
    }

    public T SetWeapon<T>() where T : Weapon
        => (T)SetWeapon(typeof(T));

    public Weapon SetWeapon(Type type)
    {
        if (!type.IsSubclassOf(typeof(Weapon)))
            throw new Exception("SetWeapon Type is not a subclass of Weapon type");

        if (Weapon) Destroy(Weapon);

        return InitWeapon((Weapon)gameObject.AddComponent(type));
    }

    private T InitWeapon<T>(T wep) where T : Weapon
    {
        wep.DefaultPreset();
        weapon = wep;
        return wep;
    }

    private void Fire(float angle)
    {
        Sounds.PlayRandom("Fire");
        cooldown = Weapon.cooldown;
        float strength = Weapon.Fire(this, angle);
        body.AddForce(angle, strength, strength);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        perks.Activate<ICollide>(1, perk => perk.OnCollide(this, collision));

        Sounds.PlayRandom("Collide");
        //Apply collision force
        for (int i = 0; i < collision.contactCount; i++)
            ApplyCollisionForce(collision.contacts[i], 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var otherPos = collision.collider.ClosestPoint(transform.position);
        var distance = (otherPos - (Vector2)transform.position).magnitude;
        if (distance == 0) TakeDamage(1, null, null);
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
            if(attacker) attacker.OnKill(this);
    }

    private void SpawnHitParticles(Vector2 position)
    {
        Sounds.PlayRandom("Hit");
        if (body && body.hitPrefab)
        {
            var debris = Instantiate(body.hitPrefab, GameModes.GetDebrisTransform(team));
            debris.transform.position = position;
            debris.transform.rotation = Quaternion.Euler(0, 0, 
                Vectors2.TrueAngle(transform.position, position));
            ParticleSystem.MainModule settings = debris.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(_colors[0]);
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

    public virtual void OnHealthChange() { }

    public virtual bool OnDeath() => true;

    public virtual void OnDeathEnded()
        => GameModes.GameMode.MemberDestroyed(this);

    public virtual void OnXPChange(bool isUp) { }

    public void OnKill(BaseController target)
    {
        AddXP(target.xp + (target.XPRequired / (level + 2) ));
        GameModes.Run<IElimination>(mode => mode.Elimenation(target, this));
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
            GameModes.Run<IControllerLevelUp>(mode =>
                mode.ControllerLevelUp(this));
        }
        OnXPChange(up);
    }
}
