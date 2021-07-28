using Scripts.OOP.EnemyBehaviors.Ability.OrbiterSpawners;
using System;
using UnityEngine;

namespace Scripts.Orbiters
{
    public abstract class Orbiter : MonoBehaviour
    {
        public BaseController Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                Color = _owner.GetColor(1);
            }
        }

        private BaseController _owner;

        public float speed = 1;
        public float velocityLoss = 0.8f;

        public float Damage = 1;

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

            if(Archetype != null)
                Target = Archetype.FindTarget(Target);

            Archetype?.Update(this);
        }

        private void Follow()
        {
            velocity *= velocityLoss;
            if (!Owner) return;
            velocity += Owner.transform.position - transform.position;

            transform.Translate(velocity * Time.deltaTime * speed);
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
#pragma warning disable IDE0083 // Use pattern matching
            if (!(Owner is EnemyController enemy)) return;

            if (!(enemy.Behavior.ability is OrbiterSpawner
                <TOrbiterType> spawner)) return;

            if (!(this is TOrbiterType spawn)) return;
#pragma warning restore IDE0083 
            spawner.OrbiterDestroyed(spawn);
        }
    }
}
