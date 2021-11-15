using Scripts.MonoBehaviors.Weapons.Other;

namespace Scripts.OOP.EnemyBehaviors.Ability.ProjectileLaunchers
{
    public class RetaliativeDefenseSystem : GameObjectSpawner<Missile>
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
            float damage = 20, float range = 10, float force = 1,
            float statsMultiplier = 0.1f,
            string projectile = "Projectiles/Missile") 
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

            if (!base.TryInstantiate(out _, out Missile missile)) return;

            missile.transform.position = self.transform.position;
            missile.Activate(damage * m, force * m, self, attacker.transform);
            missile.explosion.Range = range * m;

            charges--;
        }
    }
}
