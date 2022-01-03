using IgnitedBox.Entities;
using Scripts.Explosion;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviors.Weapons.Other
{
    public abstract class OtherProjectile : MonoBehaviour,
        ITargetEntity<ProjectileHandler>, ITargetEntity<ExplosionHandler.Effect>
    {
        public Collider2D BodyCollider => 
            bodyCollider ? bodyCollider : (bodyCollider = GetComponent<Collider2D>());
        Collider2D bodyCollider;

        protected BaseController owner;

        public int IgnoreTeam
        {
            get => owner ? owner.Team : _ignoreTeam;
            protected set
            {
                if (owner) return;
                _ignoreTeam = value;
            }
        }

        private int _ignoreTeam = -1;

        protected float damage;
        protected float force;

        protected bool active = false;

        protected virtual bool IsTrigger => false;

        private SoundHandler sounds;

        private void Start() => OnStart();

        protected virtual void OnStart() 
        {
            sounds = GetComponent<SoundHandler>();
        }

        public abstract void Activate(float damage, float force, BaseController owner);

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsTrigger) OnCollide(collision.collider);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsTrigger) OnCollide(collision);
        }

        public virtual void OnCollide(Collider2D collision)
        {
            if (!active) return;

            BaseController victim = collision.gameObject.GetComponent<BaseController>();
            if (!victim || owner.IsTeammate(victim)) return;

            OnHit(victim);
            Destroy();
        }

        protected virtual void OnHit(BaseController victim)
        {
            if (!victim) return;
            victim.TakeDamage(damage, owner, transform.position);
            victim.ApplyCollisionForce(transform.position, force, force);
        }

        protected virtual void Destroy()
        {
            if (!active) return;
            active = false;

            if (sounds) sounds.PlayRandom("Trigger");

            BodyCollider.enabled = false;
            var rdb = GetComponent<Rigidbody2D>();
            if (rdb) Destroy(rdb);
            Destroy(BodyCollider);
            var sr = GetComponent<SpriteRenderer>();
            if(sr) sr.enabled = false;
        }

        public bool Trigger(ProjectileHandler projectile)
            => TriggerExplosive();
        public bool Trigger(ExplosionHandler.Effect projectile)
            => TriggerExplosive();

        private bool TriggerExplosive()
        {
            if (!active) return false;
            OnHit(null);
            Destroy();
            return true;
        }
    }
}
