using IgnitedBox.SaveData.Databases.Sqlite;

namespace Scripts.OOP.Database
{
    public class LevelProgress : SqliteHandler.SqlTable<string>
    {
        public int TopScore { get; set; }

        public int LastScore { get; set; }
        public int LevelReached { get; set; }
    }
}
