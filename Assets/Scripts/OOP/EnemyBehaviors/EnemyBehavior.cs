using Scripts.OOP.EnemyBehaviors.Fire;
using Scripts.OOP.EnemyBehaviors.Targetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scripts.OOP.EnemyBehaviors
{
    public class EnemyBehavior
    {
        public readonly static List<Type> targets = GetBehaviors<ITargetBehavior>();
        public readonly static List<Type> firing = GetBehaviors<IFireBehavior>();

        private readonly static Dictionary<string, Type> targettingBehaviors
            = targets.ToDictionary(t => t.Name);

        private readonly static Dictionary<string, Type> firingBehaviors
            = firing.ToDictionary(t => t.Name);

        private static List<Type> GetBehaviors<IT>()
        {
            Type interfaceType = typeof(IT);
            Assembly assembly = Assembly.GetAssembly(interfaceType);
            var types = assembly.GetTypes();

            var result = types.Where(t => !t.IsAbstract && !t.IsInterface
                && interfaceType.IsAssignableFrom(t)).ToList();

            return result;
        }

        private static IT Load<IT, DefaultT>(string key,
            Dictionary<string, Type> values) where DefaultT : IT
            => (IT)Activator.CreateInstance(
                values.TryGetValue(key, out Type type) ?
                type : typeof(DefaultT));

        private readonly ITargetBehavior target;
        private readonly IFireBehavior fire;

        public EnemyBehavior(string target, string fire)
        {
            this.target = Load<ITargetBehavior, SingleTarget>
                (target, targettingBehaviors);

            this.fire = Load<IFireBehavior, TargetAim>
                (fire, firingBehaviors);
        }

        public BaseController Target(EnemyController self) 
            => target.Target(self);

        public bool Fire(EnemyController self, out float angle) 
            => fire.Fire(self, out angle);
    }
}
