using IgnitedBox.Entities;
using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.UnityUtilities.Vectors;
using IgnitedBox.Utilities;
using Scripts.Explosion;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Props;
using UnityEngine;

namespace Scripts.Items.InGame.Props.Targets
{
    public class TargetPractice : HealthEntity, ILevelProp,
        ITargetEntity<ProjectileHandler>, ITargetEntity<ExplosionHandler.Effect>
    {
        public bool Enabled { get => enabled; set => enabled = value; } 
        public bool Consumed => !gameObject || destroyed;
        private bool destroyed;

        public override float MaxHealth => maxhealth;
        private int maxhealth = 10;

        public override float Health => health;
        private float health;

        private float speed;

        private Collider2D _collider;
        private Rigidbody2D _body;

        private Vector3 startScale;

        private Vector3? target;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            if (!_collider) _collider = gameObject.AddComponent<CircleCollider2D>();

            _body = GetComponent<Rigidbody2D>();
            if (!_body) _body = gameObject.AddComponent<Rigidbody2D>();
            _body.gravityScale = 0;

            health = maxhealth;

            startScale = transform.localScale;
        }

        private void Update()
        {
            if (destroyed) return;

            Move();
        }

        private void Move()
        {
            if (speed <= 0) return;

            //No current target, get a new
            if (!target.HasValue)
            {
                float angle = Random.Range(0, 360);
                Vector2 direction = Vectors2.FromDegAngle(angle);
                const float maxDist = 100;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDist, 1<<8);
                target = hit ? hit.point : direction * maxDist;
            }

            //Find travel vector
            Vector2 distance = target.Value - transform.position;

            //Target reached
            float dist = ((10 * transform.localScale).magnitude / 2);
            if (distance.magnitude <= dist)
            {
                target = null;
                return;
            }

            //Max speed reached
            if (_body.velocity.magnitude >= speed) return;

            //Go towards target
            _body.velocity += distance * Time.deltaTime;
        }

        public override void ApplyCollisionForce(Vector2 hit, float magnitude, float push)
        {
            Vector2 dir = (Vector2)transform.position - hit;
            _body.velocity += dir * (magnitude + push);
        }

        public override bool ModifyHealth(float mod)
        {
            health += mod;

            transform.Tween<Transform, Vector3, ScaleTween>
                (startScale * (health/(maxhealth * 2))
                + (startScale / 2), 0.5f, easing: BackEasing.Out);

            if (health <= 0)
            {
                Destroy();
                return true;
            }

            return false;
        }

        public bool Trigger(ExplosionHandler.Effect effect)
        {
            if (destroyed) return false;

            float damage = effect.GetDamage();
            ModifyHealth(damage);

            (Vector2 hit, float force, _) = effect.GetHit(transform.position);

            ApplyCollisionForce(hit, 0, force);

            return true;
        }

        public bool Trigger(ProjectileHandler projectile)
        {
            if (destroyed) return false;

            if (!projectile || !projectile.active) return false;

            ModifyHealth(-projectile.damage);

            ApplyCollisionForce(projectile.transform.position, 0, projectile.force);

            return true;
        }

        public void LoadParameters(object[] param)
        {
            maxhealth = param.ParamAs(0, 10);
            speed = param.ParamAs(1, 0);
        }

        private void Destroy()
        {
            destroyed = true;

            transform.Tween<Transform, Vector3, ScaleTween>
                (Vector3.zero, 0.5f);

            Destroy(gameObject, 0.5f);

            if (GameModes.GameMode is CustomLevel level)
                level.ObjectiveEvents.Invoke<ILevelProp, string>
                    (typeof(Prop_Activation), this, gameObject.name);
        }
    }
}
