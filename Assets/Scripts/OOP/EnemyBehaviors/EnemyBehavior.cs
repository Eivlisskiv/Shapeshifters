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
        public enum Targetting { SingleTarget };

        private readonly static Dictionary<Targetting, Type> targettingBehaviors
            = GetBehaviors<Targetting, ITargetBehavior>();

        public enum Firing { TargetAim, ChargeTarget };

        private readonly static Dictionary<Firing, Type> firingBehaviors
            = GetBehaviors<Firing, IFireBehavior>();

        private static Dictionary<EnumT, Type> GetBehaviors<EnumT, IT>()
        {
            Type interfaceType = typeof(IT);
            Assembly assembly = Assembly.GetAssembly(interfaceType);
            var types = assembly.GetTypes();

            var result = types.Where(t => !t.IsAbstract && !t.IsInterface
                && interfaceType.IsAssignableFrom(t)).ToList();

            var names = (EnumT[])Enum.GetValues(typeof(EnumT));

            var dict = new Dictionary<EnumT, Type>();

            for (int i = 0; i < names.Length; i++)
            {
                EnumT en = names[i];
                string name = en.ToString();
                int found = result.FindIndex(
                    t => t.Name.Equals(name));
                if (found > -1)
                {
                    dict.Add(en, result[found]);
                    result.RemoveAt(found);
                }
            }

            return dict;
        }

        private static IT Load<EnumT, IT, DefaultT>(EnumT key,
            Dictionary<EnumT, Type> values) where DefaultT : IT
            => (IT)Activator.CreateInstance(
                values.TryGetValue(key, out Type type) ?
                type : typeof(DefaultT));

        private readonly ITargetBehavior target;
        private readonly IFireBehavior fire;

        public EnemyBehavior(Targetting target, Firing fire)
        {
            this.target = Load<Targetting, ITargetBehavior, SingleTarget>
                (target, targettingBehaviors);

            this.fire = Load<Firing, IFireBehavior, TargetAim>
                (fire, firingBehaviors);
        }

        public BaseController Target(EnemyController self) 
            => target.Target(self);

        public bool Fire(EnemyController self, out float angle) 
            => fire.Fire(self, out angle);
    }
}
