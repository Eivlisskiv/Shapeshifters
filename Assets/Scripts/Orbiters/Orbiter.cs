using IgnitedBox.Entities;
using Scripts.Explosion;
using Scripts.OOP.EnemyBehaviors.Ability.OrbiterSpawners;
using System;
using UnityEngine;

namespace Scripts.Orbiters
{
    public abstract class Orbiter : HealthEntity, 
        ITargetEntity<ProjectileHandler>, ITargetEntity<ExplosionHandler.Effect>
    {
        public BaseController Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                Color = _owner.GetColor(1);
                if (Color.a == 0) Color = _owner.GetColor(0);
                health = MaxHealth;
                Target = null;
            }
        }

        private BaseController _owner;

        public float speed = 1;
        public float velocityLoss = 0.8f;

        private float health;

        public override float Health => health;

        public override float MaxHealth => 25 + (5 * Owner.Level);

        public float damage = 1;

        public BaseController Target { get; protected set; }

        public OrbiterArchetype Archetype { get; private set; }

        protected float activeCooldown;

        public Collider2D Collider { get; private set; }

        private Vector3 velocity;

        private string ResourcePath;

        protected bool Started { get; private set; }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnColorChange();
            }
        }

        private Color _color;

        protected virtual bool CanBeStolen => true;

        private void Awake()
        {
            ResourcePath = $"Orbiters/{GetType().Name}/";
        }

        public T LoadResource<T>(string name) where T : UnityEngine.Object
            => Resources.Load<T>(ResourcePath + name);

        // Start is called before the first frame update
        void Start()
        {
            if (Started) return;
            OnStart();
            Started = true;
        }

        protected virtual void OnStart()
        {
            Collider = GetComponent<Collider2D>();
            if (!Collider) Collider = gameObject.AddComponent<PolygonCollider2D>();
            Collider.isTrigger = true;
        }

        // Update is called once per frame
        void Update() => OnUpdate();

        protected virtual void OnUpdate()
        {
            if (!Owner) Destroy(gameObject);

            Follow();

            if (Archetype == null) return;

            Target = Archetype.FindTarget(Target);
            Archetype.Update(this);
        }

        private void Follow()
        {
            velocity *= velocityLoss;
            if (!Owner) return;
            velocity += Owner.transform.position - transform.position;

            transform.Translate(velocity * Time.deltaTime * speed);
        }

        public override bool ModifyHealth(float mod)
        {
            if (mod < 0) Target = null;
            return (health += mod) <= 0;
        }

        public bool Trigger(ProjectileHandler projectile)
        {
            if (projectile.IsSameSender(Owner.gameObject)) return false;

            if (projectile.Sender.IsTeammate(Owner)) return false;

            if (ModifyHealth(-projectile.damage) && CanBeStolen)
            {
                OnOrbiterDestroy();
                Owner = projectile.Sender;
            }

            return true;
        }

        public bool Trigger(ExplosionHandler.Effect effect)
        {
            if (effect.Teammate(Owner.Team)) return false;

            if (ModifyHealth(-effect.GetDamage()/2) && !CanBeStolen)
            {
                OnOrbiterDestroy();
            }

            return true;
        }

        protected virtual void OnColorChange()
            => Archetype?.SetColor(Color);

        public void SetArchetype<T>()
            where T : OrbiterArchetype
            => SetArchetype(typeof(T));

        public void SetArchetype(Type archetype)
        {
            if (archetype == null || archetype.IsAbstract
                || !archetype.IsSubclassOf(typeof(OrbiterArchetype)))
                archetype = RandomArchetype();

            SetArchetype((OrbiterArchetype)Activator.CreateInstance(archetype));
        }

        protected abstract Type RandomArchetype();

        public void SetArchetype(OrbiterArchetype value)
        {
            if (!Started)
            {
                OnStart();
                Started = true;
            }

            Archetype?.OnRemove();

            Archetype = value;
            Archetype?.Start(this);
        }

        private void OnDestroy()
        {
            if (!Owner) return;

            OnOrbiterDestroy();
        }

        protected abstract void OnOrbiterDestroy();

        protected void CheckOrbiterSpawner<TOrbiterType>()
            where TOrbiterType : Orbiter
        {
            if (!Owner) return;

#pragma warning disable IDE0083 // Use pattern matching
            if (!(Owner is EnemyController enemy)) return;

            if (!(enemy.Behavior.ability is OrbiterSpawner
                <TOrbiterType> spawner)) return;

            if (!(this is TOrbiterType spawn)) return;
#pragma warning restore IDE0083 
            spawner.OrbiterLost(spawn);
        }

        public override void ApplyCollisionForce(Vector2 hit, float magnitude, float push)
        {
            velocity += ((Vector3)hit * magnitude * push);
        }
    }
}
