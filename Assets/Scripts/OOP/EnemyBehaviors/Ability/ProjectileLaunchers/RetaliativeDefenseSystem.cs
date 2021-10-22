using Assets.IgnitedBox.Entities;
using Scripts.OOP.ProjectileFunctions;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability.ProjectileLaunchers
{
    public class RetaliativeDefenseSystem : ProjectileLauncher
    {
        readonly float cooldownTime;

        readonly int maxCharges;

        readonly float damage;
        readonly float range;
        readonly float force;

        readonly float statsMultiplier;

        private float cooldown;

        private int charges;

        public RetaliativeDefenseSystem(float cooldownTime = 15, int maxCharges = 3,
            float damage = 20, float range = 10, float force = 20,
            float statsMultiplier = 0.1f,
            string projectile = "Missile") 
            : base(projectile) 
        {
            this.cooldownTime = cooldownTime;
            cooldown = cooldownTime;
            this.maxCharges = maxCharges;

            this.damage = damage;
            this.range = range;
            this.force = force;
            this.statsMultiplier = statsMultiplier;
        }

        public override void Initialize(BaseController self)
        {
            self.Events.Subscribe<BaseController, BaseController>
                (BaseController.ControllerEvents.DamageTaken, Retaliate);

            self.Events.Subscribe<BaseController, float>
                (BaseController.ControllerEvents.Update, Charge);
        }

        protected virtual void Charge(BaseController self, float delta)
        {
            if (charges >= maxCharges) return;

            if(cooldown > 0)
            {
                cooldown -= delta;
                return;
            }

            charges++;
            cooldown = cooldownTime;
        }

        protected virtual void Retaliate(BaseController self, BaseController attacker)
        {
            if (charges <= 0) return;

            float m = 1 + (statsMultiplier * self.Level);

            ProjectileHandler projectile = Fire(self, m * damage, m * range, 
                (attacker.transform.position - self.transform.position)
                .normalized, m * force);

            if (!projectile) return;

            projectile.active = false;
            projectile.OnUpdate = ProjectileUpdate.Missile(attacker.transform);

            projectile.body.transform.GetComponent<SpriteRenderer>().color = Color.white;
            projectile.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }

        protected override void OnProjectileHit(ProjectileHandler projectile, Collider2D collider)
        {
            if (!projectile.active) return;

            if (projectile.IsSameSender(collider.gameObject)) return;

            if(!HealthEntity<ProjectileHandler>.HasHeathEntity
                (collider.gameObject, projectile)) return;

            projectile.ToDestroy();
        }

        protected override void OnProjectileUpdate(ProjectileHandler projectile) { }
    }
}
