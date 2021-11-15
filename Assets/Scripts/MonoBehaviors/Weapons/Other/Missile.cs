using Assets.Scripts.MonoBehaviors.Weapons.Other;
using Scripts.Explosion;
using UnityEngine;

namespace Scripts.MonoBehaviors.Weapons.Other
{
    public class Missile : OtherProjectile
    {
        public Transform spinner;

        public GameObject body;
        
        public FireworkExplosionHandler explosion;

        private Transform target;
        Vector3 velocity;

        private void Update()
        {
            if (!active) return;
            Move();
            Spin();
        }

        private void Spin()
        {
            spinner.rotation = Quaternion.Euler(spinner.rotation.eulerAngles + new Vector3(0, 0, velocity.magnitude / 5));
        }

        private void Move()
        {
            Vector3 diff = (target.position - transform.position).normalized * 3.5f;
            velocity += diff;

            transform.position += velocity * Time.deltaTime;

            velocity *= 0.9f;
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
                explosion.ignoreTeam = owner.Team;
                Physics2D.IgnoreCollision(BodyCollider, owner.Body.Collider);
                body.GetComponent<SpriteRenderer>().color = owner.GetColor(0);
            }

            BodyCollider.enabled = true;
        }

        protected override void OnCollide(Collider2D collision)
        {
            if (!active || collision.gameObject == owner.gameObject) return;

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

            Destroy(body);
            Destroy(gameObject, explosion.Duration + 0.5f);
        }
    }
}
