using Scripts.OOP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scripts.OOP.Perks
{
    public class PerksHandler
    {
        public static readonly Dictionary<string, Type> perksTypes 
            = LoadPerksTypes();

        private static Dictionary<string, Type> LoadPerksTypes()
        {
            Type perk = typeof(Perk);
            Assembly assembly = Assembly.GetAssembly(perk);
            var types = assembly.GetTypes();

            var result = types.Where(t => t.IsSubclassOf(perk)
            && !t.IsAbstract && !t.IsInterface).ToDictionary(
                t => t.Name.Replace('_', ' '));
            return result;
        }

        public static Perk Random()
        {
            string key = perksTypes.Keys.ElementAt(UnityEngine.Random
                .Range(0, perksTypes.Keys.Count));
            return Load(key);
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
            if (perk.ui) UnityEngine.Object.Destroy(perk.ui.gameObject);
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
                }
            }
        }

        public Perk RandomDrop()
        {
            Perk perk = Randomf.Element(perks);
            perk.ToBuff();
            return perk;
        }
    }
}
