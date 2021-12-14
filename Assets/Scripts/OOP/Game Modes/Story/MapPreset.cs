using IgnitedBox.UnityUtilities.Vectors;
using Scripts.OOP.TileMaps;
using System;

namespace Scripts.OOP.Game_Modes.Story
{
    [Serializable]
    public class StoryLevel
    {
        public string name;
        public SerializableInt2 playerSpawn;
        public MapPreset[] maps;
        public ObjectiveData[] main;
        //public ObjectiveData[] secondary; Seconday will have conditions?
    }

    [Serializable]
    public class MapPreset
    {
        public int tileBaseIndex;
        public MapTileType[,] tiles;
        public MapProp[] props;
    }

    [Serializable]
    public struct MapProp
    {
        public string id;
        public string prefabPath;
        public SerializableFloat2 position;
    }

    [Serializable]
    public struct ObjectiveData
    {
        public string id;
        public SerializableFloat4 color;
        public SerializableFloat2? track;
        public object[] parameters;
    }
}
