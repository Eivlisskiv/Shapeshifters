using IgnitedBox.UnityUtilities.Vectors;
using Scripts.OOP.TileMaps;
using System;

namespace Scripts.OOP.Game_Modes.Story
{
    [Serializable]
    public struct StoryLevel
    {
        public int chapter;
        public int act;
        public SerializableVector2Int playerSpawn;
        public int tileBase;
        public MapPreset[] maps;
    }

    [Serializable]
    public struct MapPreset
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
        public SerializableVector2 position;
    }
}
