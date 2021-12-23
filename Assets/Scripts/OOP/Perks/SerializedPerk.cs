using IgnitedBox.SaveData.Databases.Sqlite;

namespace Scripts.OOP.Perks
{
    [SqliteHandler.AutoIncrement]
    public class SerializedPerk : SqliteHandler.SqlTable<int>
    {
        public string LevelKey { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Buff { get; set; }
        public float Charge { get; set; }

        public SerializedPerk() { }

        public SerializedPerk(string storyId, Perk perk)
        {
            LevelKey = storyId;
            Name = perk.Name;
            Level = perk.Level;
            Buff = perk.Buff;
            Charge = perk.Charge;
        }
    }
}
