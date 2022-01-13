using Assets.Scripts.MonoBehaviors.Weapons.Other;
using Scripts.Explosion;
using UnityEngine;

namespace Scripts.MonoBehaviors.Weapons.Other
{
    public class Missile : OtherProjectile
    {
        const float speed = 12f;
        const float rotSpeed = speed / 4;

        private ParticleSystem[] particles;

        public GameObject body;
        
        public FireworkExplosionHandler explosion;

        public Rigidbody2D RigidBody
        {
            get
            {
                if (!_rigidBody) _rigidBody = GetComponent<Rigidbody2D>();
                return _rigidBody;
            }
        }
        private Rigidbody2D _rigidBody;

        private Transform target;

        private void Update()
        {
            if (!active) return;
            Spin();
            Move();
            
        }

        private void Spin()
        {
            Transform spin = body.transform;
            spin.rotation = Quaternion.Euler(spin.rotation.eulerAngles + new Vector3(0, 0, RigidBody.velocity.magnitude * rotSpeed));
        }

        private void Move()
        {
            if (!target) return;

            Vector2 diff = (target.position - transform.position).normalized * speed;
            RigidBody.velocity += diff * Time.deltaTime;

        }

        public void SetColor(Color color)
        {
            if (particles == null)
            {
                particles = new ParticleSystem[3];
                particles[0] = body.GetComponent<ParticleSystem>();
                for (int i = 1; i < particles.Length; i++)
                {
                    var a = body.transform.GetChild(i - 1);
                    particles[i] = a.GetComponent<ParticleSystem>();
                }
            }


            for (int i = 0; i < particles.Length; i++)
            {
                var main = particles[i].main;
                main.startColor = color;
            }
        }

        public void Activate(float damage, float force, BaseController owner, Transform target)
        {
            this.target = target;
            Activate(damage, force, owner);
        }

        public override void Activate(float damage, float force, BaseController owner)
        {
            explosion.damage = damage;
            explosion.Intensity = force;
            this.owner = owner;
            active = true;

            if (owner)
            {
                explosion.Sender = owner;
                Physics2D.IgnoreCollision(BodyCollider, owner.Body.Collider);
                SetColor(owner.GetColor(0));
            }

            BodyCollider.enabled = true;
        }

        public override void OnCollide(Collider2D collision)
        {
            if (!active || (owner && collision.gameObject == owner.gameObject)) return;

            OnHit(null);
            
            Destroy();
        }

        protected override void OnHit(BaseController victim)
        {
            explosion.Play();
        }

        protected override void Destroy()
        {
            base.Destroy();

            for (int i = 0; i < particles.Length; i++)
                particles[i].Stop();

            Destroy(body, explosion.Duration + 0.5f);
            Destroy(gameObject, explosion.Duration + 0.5f);
        }
    }
}
