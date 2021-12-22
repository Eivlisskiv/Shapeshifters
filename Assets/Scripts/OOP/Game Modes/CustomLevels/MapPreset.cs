using IgnitedBox.UnityUtilities.Vectors;
using System;

namespace Scripts.OOP.Game_Modes.Story
{
    [Serializable]
    public class LevelSettings
    {
        public string id { get; set; }

        public string Creator { get; set; }

        public string name { get; set; }

        public int Chapter { get; set; }
        public int Episode { get; set; }

        public string description { get; set; }
        public SerializableInt2 playerSpawn { get; set; }
        public MapPreset[] maps { get; set; }
        public ObjectiveData[] main { get; set; }
        //public ObjectiveData[] secondary; Seconday will have conditions?
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
