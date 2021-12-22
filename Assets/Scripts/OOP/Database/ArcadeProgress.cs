using IgnitedBox.SaveData.Databases.Sqlite;

namespace Scripts.OOP.MongoRealm
{
    public class ArcadeProgress : SqliteHandler.SqlTable<string>
    {
        public int TopScore { get; set; }
        public int LastScore { get; set; }
    }
}
