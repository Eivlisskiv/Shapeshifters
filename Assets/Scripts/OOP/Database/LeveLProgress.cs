using IgnitedBox.SaveData.Databases.Sqlite;
using UnityEngine;

namespace Scripts.OOP.Database
{
    public class LevelProgress : SqliteHandler.SqlTable<string>
    {
        public int TopScore { get; set; }

        public float BestTimeSeconds { get; set; }

        public int LevelReached { get; set; }

        public string BestTime()
        {
            float seconds = BestTimeSeconds;
            int minutes;
            int hours = (int)(seconds / 3600);

            seconds -= hours * 3600;
            minutes = (int)(seconds / 60);
            seconds = Mathf.Round((seconds - (minutes * 60)) * 100) / 100;

            string hs = hours <= 0 ? null : 
                (hours < 10 ? $"0{hours}:" : $"{hours}:");

            string ms = minutes <= 0 ? null :
                (minutes < 10 ? $"0{minutes}:" : $"{minutes}:");

            string ss = seconds < 10 ? $"0{seconds}" : $"{seconds}";

            return hs + ms + ss;
        }
    }
}
