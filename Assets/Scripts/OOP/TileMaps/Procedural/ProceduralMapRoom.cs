using UnityEngine;

namespace Scripts.OOP.TileMaps.Procedural
{
    public abstract class ProceduralMapRoom : MapRoom
    {
        public const int spacing = 8;
        public const int borderWidth = 3;
        public const int segment = 10;

        public static int RandomSize()
        {
            int min = (spacing / 2);
            return Random.Range(min, min * 3) * segment;
        }

        public Vector2Int previousBorder;

        public abstract int TileBaseIndex { get; }

        public ProceduralMapRoom(Vector2Int size, RoomHandler previous,
            Transform propsContainer)
            : base(previous, size, propsContainer)
        {
            mapContent = new MapTileType[size.x + 1, size.y + 1];

            //if (previous) previousBorder = previous.current;
            /*else*/
            previousBorder = new Vector2Int(0, size.y);
        }

        public override void LoadFinished() { } 
    }
}
