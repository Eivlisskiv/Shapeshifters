﻿using IgnitedBox.Entities;
using IgnitedBox.EventSystem;
using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.UnityUtilities.Vectors;
using Scripts.Explosion;
using Scripts.OOP.Character.Stats;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks;
using Scripts.OOP.Perks.Character.Triggers;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.UI.StatsBar;
using System;
using UnityEngine;

public abstract class BaseController : HealthEntity, 
    ITargetEntity<ProjectileHandler>, ITargetEntity<ExplosionHandler.Effect>
{
    public enum ControllerEvents 
    { 
        Update, Fired, Collide,
        DamageTaken
    }

    public Weapon Weapon => weapon ? weapon : (weapon = GetComponent<Weapon>());

    private Weapon weapon;

    public BodyPhysicsHandler Body 
        => _body != null ? _body : 
        (_body = GetComponent<BodyPhysicsHandler>()); 
    private BodyPhysicsHandler _body;

    internal int Team { get; set; }

    public virtual string Name
    {
        get => name;
        set => name = value;
    }

    public SoundHandler Sounds 
    { 
        get
        {
            if (!_sounds)  _sounds = GetComponent<SoundHandler>();
            return _sounds;
        }
    }
    private SoundHandler _sounds;

    public ShieldHealthBar HealthBar { get; private set; }

    float cooldown = 0;
    public bool FireReady => cooldown <= 0;
    public Stats stats;

    internal EventsHandler<ControllerEvents> Events;
    internal PerksHandler perks;

    public int Level { get; protected set; }

    public float Xp { get; protected set; }
    public int XPRequired => Mathf.FloorToInt((Level + 1) * (float)Math.Pow(10, 2));

    private readonly Color[] _colors = new Color[2];
    public void SetColor(int i, Color value)
    {
        _colors[i] = value;
        if (Body) Body.SetColor(i, value);
    }
    public Color GetColor(int i) => _colors[i];

    protected bool ControllerEnabled { get; private set; } = true;
    protected float? Dying { get; private set; } = null;

    // Start is called before the first frame update
    void Start()
    {
        Events = new EventsHandler<ControllerEvents>();
        Body.SetColor(0, _colors[0]);
        Body.SetColor(1, _colors[1]);

        ScaleSpawn();

        OnStart();

        if (stats == null) stats = new Stats(30 + (Level / 5));
        if (perks == null) perks = new PerksHandler();
    }

    public void ScaleSpawn()
    {
        transform.localScale = Vector3.zero;
        transform.Tween<Transform, Vector3, ScaleTween>(Vector2.one, 2, easing: ElasticEasing.Out);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckDying()) return;

        if (!ControllerEnabled) return;

        OnUpdate();
        Events.Invoke(ControllerEvents.Update, this, Time.deltaTime);

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

    public override float Health => stats.health;

    public override float MaxHealth => stats.MaxHealth;

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract bool IsFiring(out float angle);

    public void DisableController(bool value)
        => ControllerEnabled = !value;

    private bool CheckDying()
    {
        if (Dying.HasValue)
        {
            Dying = Math.Max(0, Dying.Value - Time.deltaTime);
            if (Dying > 0)
            {
                Body.Body.velocity = Vector2.zero;
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 1), Time.deltaTime);
            }
            else
            {
                transform.localScale = Vector3.zero;
                var debris = Instantiate(Body.deathPrefab);
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

    public virtual void AddPerk(Perk perk)
        => perks.Add(perk);

    private void Fire(float angle)
    {
        Sounds.PlayRandom("Fire");
        cooldown = Weapon.cooldown;
        (float strength, ProjectileHandler projectile) = Weapon.Fire(this, angle);
        Body.AddForce(angle, strength, strength);

        Events.Invoke(ControllerEvents.Fired, this, projectile);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) return;

        perks.Activate<ICollide>(1, perk => perk.OnCollide(this, collision));

        Events.Invoke(ControllerEvents.Collide, this, collision);

        Sounds.PlayRandom("Collide");
        //Apply collision force
        for (int i = 0; i < collision.contactCount; i++)
            ApplyCollisionForce(collision.contacts[i], 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var otherPos = collision.collider.ClosestPoint(transform.position);
        var outer = otherPos - (Vector2)transform.position;
        if (outer.magnitude == 0)
        {
            if(collision.collider.TryGetComponent(out Rigidbody2D body)
                && !body.isKinematic)
            {
                transform.position += (Vector3)outer.normalized * Time.deltaTime;
                return;
            }

            GameModes.GameMode?.RespawnController(this);
        }
    }

    public bool Trigger(ProjectileHandler projectile)
    {
        perks.Activate<IProjectileTaken>(1,
            perk => perk.OnHit(this, projectile));

        if (!projectile || !projectile.active) return false;

        float push = projectile.force;
        if (!IsTeammate(projectile.Sender))
        {
            projectile.Sender.perks.Activate<IProjectileHitTarget>(1,
                perk => perk.OnHit(projectile, this));

            ProcessDamage(projectile.damage, projectile.Sender,
                projectile.transform.position);
        }

        ApplyCollisionForce(projectile.transform.position, 0, push);

        return true;
    }

    public bool Trigger(ExplosionHandler.Effect effect)
    {
        (Vector2 hit, float magnitude, float push) = effect.GetHit(transform.position);
        ApplyCollisionForce(hit, magnitude, push);

        if (effect.Teammate(Team)) return true;

        float damage = effect.GetDamage();

        perks.Activate<IReceiveDamage>(1,
                perk => perk.OnHit(this, hit, ref damage));

        ProcessDamage(damage, effect.Sender, hit);

        return true;
    }

    public bool IsTeammate(BaseController other)
        => GetType().Equals(other.GetType());

    private void ApplyCollisionForce(ContactPoint2D contact, float push)
        => ApplyCollisionForce(contact.point,
            contact.relativeVelocity.magnitude, push);
    public override void ApplyCollisionForce(Vector2 hit, float magnitude, float push)
    {
        Vector2 pos = transform.position;
        int y = pos.y > hit.y ? -1 : 1;
        float angle = Vector2.Angle(Vector2.left, pos - hit) * y;
        /*if (push > 2)*/ Body.AddForce(angle, magnitude, push);
    }

    public void TakeDamage(float damage, BaseController attacker, Vector2? direction)
    {
        perks.Activate<IReceiveDamage>(1,
                perk => perk.OnHit(this, direction, ref damage));

        if(damage > 0)
            ProcessDamage(damage, attacker, direction);
        //

        if (direction.HasValue) SpawnHitParticles(direction.Value);
        if (ModifyHealth(-damage))
            if(attacker) attacker.OnKill(this);
    }

    public void ProcessDamage(float damage, BaseController attacker, Vector2? direction)
    {
        if (direction.HasValue) SpawnHitParticles(direction.Value);
        if (ModifyHealth(-damage) && attacker) attacker.OnKill(this);

        Events.Invoke(ControllerEvents.DamageTaken, this, attacker);
    }

    private void SpawnHitParticles(Vector2 position)
    {
        Sounds.PlayRandom("Hit");
        if (Body && Body.hitPrefab)
        {
            var debris = Instantiate(Body.hitPrefab, GameModes.GetDebrisTransform(Team));
            debris.transform.position = position;
            debris.transform.rotation = Quaternion.Euler(0, 0, 
                Vectors2.TrueAngle(transform.position, position));
            ParticleSystem.MainModule settings = debris.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(_colors[0]);
            Destroy(debris, 0.5f);
        }
    }

    public override bool ModifyHealth(float value)
    {
        stats.health = Math.Min(stats.MaxHealth, stats.health + value);
        OnHealthChange();
        if (!Dying.HasValue && stats.health <= 0 && OnDeath())
        {
            Dying = 1.5f;
            Body.Body.velocity = Vector2.zero;
            Body.Body.angularVelocity = 720;
            Body.Collider.enabled = false;
            return true;
        }
        return false;
    }

    public void SetHealthBar(Transform container)
    {
        HealthBar = new ShieldHealthBar(container);
    }

    protected void UpdateHealthBar()
        => HealthBar?.SetHealth(stats.HPP);

    public void UpdateShieldBar(float f)
        => HealthBar?.SetShield(f);

    public virtual void OnHealthChange()
    {
        UpdateHealthBar();
    }

    public virtual bool OnDeath() => true;

    public virtual void OnDeathEnded()
        => GameModes.GameMode.MemberDestroyed(this);

    public virtual void OnXPChange(bool isUp) { }

    public void OnKill(BaseController target)
    {
        AddXP(target.Xp + (target.XPRequired / (Level + 2) ));
        GameModes.Run<IElimination>(mode => mode.Elimination(target, this));
    }

    private void AddXP(float amount)
    {
        Xp += amount;
        bool up = false;
        while(Xp >= XPRequired)
        {
            Xp -= XPRequired;
            up = true;
            Level++;
            ModifyHealth(stats.MaxHealth - stats.health);
        }

        if (up)
        {
            GameModes.Run<IControllerLevelUp>(mode =>
                mode.ControllerLevelUp(this));
        }
        OnXPChange(up);
    }
}
