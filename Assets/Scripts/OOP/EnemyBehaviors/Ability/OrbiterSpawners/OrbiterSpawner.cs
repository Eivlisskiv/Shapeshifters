using Scripts.Orbiters;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability.OrbiterSpawners
{
    public abstract class OrbiterSpawner<TOrbiterType> : IAbilitybehavior
        where TOrbiterType : Orbiter
    {
        private readonly float cooldowntime;

        private readonly TOrbiterType[] orbiters;

        private int charges;
        private float cooldown;
        private readonly float damage;

        private readonly System.Type archetype; 

        protected OrbiterSpawner(int charges = 3, int amount = 1, 
            float damage = 1, float cooldown = 60, 
            System.Type archetype = null)
        {
            orbiters = new TOrbiterType[amount];
            this.charges = charges;
            cooldowntime = cooldown;
            this.damage = damage;

            this.archetype = archetype != null && (
                !archetype.IsSubclassOf(typeof(OrbiterArchetype))
                || archetype.IsAbstract) ? 
                null : archetype;
        }

        public void Initialize(BaseController self)
        {
            self.Events.Subscribe<BaseController, float>(BaseController.ControllerEvents.Update, CheckSpawn);
        }

        public void CheckSpawn(BaseController self, float deltaTime)
        {
            if (charges == 0) return;

            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                return;
            }

            for (int i = 0; i < orbiters.Length; i++)
            {
                TOrbiterType orbiter = orbiters[i];
                if(!orbiter || orbiter == null)
                {
                    orbiters[i] = SpawnOrbiter(self);
                    cooldown = cooldowntime;
                    return;
                }
            }
        }

        private TOrbiterType SpawnOrbiter(BaseController self)
        {
            GameObject orbiterObject = new GameObject($"{typeof(TOrbiterType).Name}");
            TOrbiterType orbiter = orbiterObject.AddComponent<TOrbiterType>();
            orbiter.Owner = self;
            orbiter.damage = damage;

            orbiter.SetArchetype(archetype);

            charges--;

            return orbiter;
        }

        public virtual void OrbiterLost(TOrbiterType _)
        {
            cooldown = cooldowntime;
        }
    }
}
