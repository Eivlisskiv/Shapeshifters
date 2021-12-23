using Scripts.OOP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scripts.OOP.Perks
{
    public class PerksHandler
    {
        private static readonly List<Type> types = LoadPerksTypes();

        public static readonly Dictionary<string, Type> perksTypes 
            = types.ToDictionary(t => t.Name.Replace('_', ' '));

        private static List<Type> LoadPerksTypes()
        {
            Type perk = typeof(Perk);
            Assembly assembly = Assembly.GetAssembly(perk);
            var types = assembly.GetTypes();

            var result = types.Where(t => !t.IsAbstract && !t.IsInterface 
                && t.IsSubclassOf(perk)).ToList();
            return result;
        }

        public static Perk Random(int level = 1)
        {
            Type type = Randomf.RandomElement(types);
            Perk perk = (Perk)Activator.CreateInstance(type);
            perk.LevelUp(level);
            return perk;
        }

        public static Perk RandomCharge(int buff, int chargeMultiplier)
        {
            Type type = Randomf.RandomElement(types);
            Perk perk = (Perk)Activator.CreateInstance(type);
            perk.AsBuff(buff, chargeMultiplier);
            return perk;
        }

        public static Perk Load(string key)
            => perksTypes.TryGetValue(key, out Type type) ?
                (Perk)Activator.CreateInstance(type) : null;



        readonly List<Perk> perks;
        public int Count => perks.Count;

        public PerksHandler()
        {
            perks = new List<Perk>();
        }

        public void Activate<T>(float consume, Func<T, bool> effect)
        {
            int i = 0;
            while (i < perks.Count)
            {
                Perk abstractPerk = perks[i];
                //If is current trigger + effect demands consume
                if (abstractPerk is T perk && effect(perk)
                    && abstractPerk.Consume(consume)) // + Perk expired
                        Remove(abstractPerk);
                else i++;
            }
        }

        private void Remove(Perk perk)
        {
            if (perk.ui) perk.ui.Remove();
            perks.Remove(perk);
        }

        public void Add(Perk perk, CharacterUIHandler ui = null)
        {
            Perk existing = perks.Find(p => p.GetType().Equals(perk.GetType()));
            if (existing == null)
            {
                if (ui) ui.AddPerk(perk);
                perks.Add(perk);
            }
            else 
            { 
                existing.Add(perk);
                if (existing.ui)
                {
                    if (perk.Buff > 0)
                        existing.ui.SetBuff(existing.Intensity, existing.Charge);
                    else existing.ui.SetLevel(existing.Level);

                    existing.ui.Bounce();
                }
            }
        }

        public Perk RandomDrop()
        {
            Perk perk = Randomf.RandomElement(perks);
            perk.ToBuff();
            return perk;
        }

        internal SerializedPerk[] Serialize(string id)
        {
            SerializedPerk[] serialized = new SerializedPerk[perks.Count];
            for (int i = 0; i < perks.Count; i++)
            {
                serialized[i] = new SerializedPerk(id, perks[i]);
            }
            return serialized;
        }
    }
}
