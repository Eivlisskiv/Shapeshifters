using Scripts.OOP.Perks;

namespace Scripts.OOP.Database
{
    public class StoryProgress : LevelProgress
    {
        public static StoryProgress Load(int chapter, int episode)
        {
            string id = $"IBS/{chapter}.{episode}";
            return LoadOne<StoryProgress>(id, true);
        }

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
    }
}
