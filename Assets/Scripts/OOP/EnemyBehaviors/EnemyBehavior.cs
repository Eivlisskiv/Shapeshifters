using Scripts.OOP.EnemyBehaviors.Ability;
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
        public readonly static Dictionary<string, Type> targettingBehaviors
            = GetBehaviors<ITargetBehavior>().ToDictionary(t => t.Name);

        public readonly static Dictionary<string, Type> firingBehaviors
            = GetBehaviors<IFireBehavior>().ToDictionary(t => t.Name);

        public readonly static Dictionary<string, Type> abilityBehaviors
            = GetBehaviors<IAbilitybehavior>().ToDictionary(t => t.Name);

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

        public readonly ITargetBehavior target;
        public readonly IFireBehavior fire;
        public readonly IAbilitybehavior ability;

        public EnemyBehavior(string target, string fire,  string ability)
        {
            this.target = Load<ITargetBehavior, SingleTarget>
                (target, targettingBehaviors);

            this.fire = Load<IFireBehavior, TargetAim>
                (fire, firingBehaviors);

            this.ability = Load<IAbilitybehavior, NoAbility>
                (ability, abilityBehaviors);
        }

        public BaseController Target(EnemyController self) 
            => target.Target(self);

        public bool Fire(EnemyController self, out float angle) 
            => fire.Fire(self, out angle);
    }
}
