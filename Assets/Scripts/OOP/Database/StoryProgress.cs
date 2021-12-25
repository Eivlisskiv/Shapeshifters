using Scripts.OOP.Perks;
using System.Collections.Generic;

namespace Scripts.OOP.Database
{
    public class StoryProgress : LevelProgress
    {
        public static readonly Dictionary<string, StoryProgress> cache
            = new Dictionary<string, StoryProgress>();

        public static bool Completed(int chapter, int episode)
            => Load(chapter, episode, false) != null;

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

        public float Experience { get; set; }

        public SerializedPerk[] Perks
        {
            get
            {
                if(_perks == null)
                    _perks = LoadAll<SerializedPerk, string>(Id, "LevelKey");
                return _perks;
            }

            set
            {
                DeleteAll<SerializedPerk, string>(Id, "LevelKey");
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
    }
}
