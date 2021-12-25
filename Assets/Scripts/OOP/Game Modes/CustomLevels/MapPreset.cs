using IgnitedBox.SaveData.Databases.Sqlite;
using IgnitedBox.UnityUtilities.Vectors;
using System;

namespace Scripts.OOP.Game_Modes.CustomLevels
{
    public class StorySettings : LevelSettings
    {
        public int Chapter { get; set; }
        public int Episode { get; set; }

        public StorySettings() : base() { }
        public StorySettings(int chapter, int episode, string name) : base(name)
        {
            Chapter = chapter;
            Episode = episode;

            Id = $"{Creator}/{chapter}.{episode}";
        }
    }

    [Serializable]
    public class LevelSettings : SqliteHandler.SqlTable<string>
    {
        public string Creator { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public SerializableInt2 PlayerSpawn { get; set; }
        public MapPreset[] Maps { get; set; }
        public ObjectiveData[] Main { get; set; }
        
        public LevelSettings() { }

        public LevelSettings(string creator, string name)
        {
            Creator = creator;
            Name = name;

            Id = $"{creator}/{name}";
        }

        protected LevelSettings(string name)
        {
            Creator = "IBS";
            Name = name;
        }
    }

    [Serializable]
    public class MapPreset
    {
        public int tileBaseIndex;
        public int[,] tiles;
        public MapProp[] props;
    }

    [Serializable]
    public class MapProp
    {
        public string id;
        public string prefabPath;
        public SerializableFloat2 position;
        public object[] parameters;

        public MapProp() { }
        public MapProp(string id, string path, float x, float y, params object[] parms) 
        {
            this.id = id;
            prefabPath = path;
            position = new SerializableFloat2(x, y);
            parameters = parms;
        }
    }

    [Serializable]
    public class ObjectiveData
    {
        public string id;
        public SerializableFloat4 color;
        public SerializableFloat2 track;
        public object[] parameters;
    }
}
