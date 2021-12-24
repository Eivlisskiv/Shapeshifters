using Scripts.OOP.Perks;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Database
{
    public class StoryProgress : LevelProgress
    {
        public static readonly Dictionary<string, StoryProgress> cache
            = new Dictionary<string, StoryProgress>();

        public static StoryProgress Load(int chapter, int episode, bool create)
        {
            string id = $"IBS/{chapter}.{episode}";

            if (cache.TryGetValue(id, out StoryProgress data)) return data;

            StoryProgress prog = LoadOne<StoryProgress>(id, create);
            if (prog != null)
            {
                if (cache.ContainsKey(id)) cache[id] = prog;
                else cache.Add(id, prog);
            }

            return prog;
        }

        public float BestTimeSeconds { get; set; }

        public float Experience { get; set; }

        public SerializedPerk[] Perks
        {
            get
            {
                if(_perks == null)
                    _perks = SerializedPerk.LoadAll<SerializedPerk, string>(Id, "LevelKey");
                return _perks;
            }

            set
            {
                SerializedPerk.DeleteAll<SerializedPerk, string>(Id, "LevelKey");
                _perks = value;
                for (int i = 0; i < _perks.Length; i++) _perks[i].Save();
            }
        }

        private SerializedPerk[] _perks;

        public string Weapon { get; set; }

        public override void Save()
        {
            base.Save();

            if (cache.ContainsKey(Id)) cache[Id] = this;
            else cache.Add(Id, this);
        }

        public string BestTime()
        {
            float seconds = BestTimeSeconds;
            int minutes;
            int hours = (int)(seconds / 3600);

            seconds -= hours * 3600;
            minutes = (int)(seconds / 60);
            seconds = Mathf.Round((seconds - (minutes * 60)) * 100) / 100;

            return hours > 0 ? $"{hours}:{minutes}:{seconds}"
                : minutes > 0 ? $"{minutes}:{seconds}"
                : $"{seconds}";
        }
    }
}
