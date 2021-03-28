using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.OOP.TileMaps
{
    public abstract class MapRoom
    {
        public const int spacing = 8;
        public const int borderWidth = 3;
        public const int segment = 10;

        public static int RandomSize()
        {
            int min = (spacing / 2);
            return Random.Range(min, min * 3) * segment;
        }

        protected Vector2Int start;
        public Vector2Int Start
        {
            get => start;
        }

        protected Vector2Int size;
        public Vector2Int Size
        {
            get => size;
        }

        protected Vector2Int current;

        public Vector2Int previousBorder;

        public MapTileType[,] mapContent;

        protected bool entrance;

        public MapRoom(Vector2Int start, Vector2Int size, bool entrance)
        {
            this.entrance = entrance;
            current = Vector2Int.zero;
            this.start = start;
            this.size = size;
            mapContent = new MapTileType[size.x + 1, size.y + 1];
            previousBorder = new Vector2Int(0, size.y);
            Initialize();
        }

        public abstract void Initialize();
        public abstract bool DrawOne(Tilemap map, TileBase tilebase, out MapTileType tile);
        public abstract bool DrawNext(Tilemap map, TileBase tile);
        public abstract bool DrawAmount(int amount, Tilemap map, TileBase tile);
        public abstract void OpenGate(bool open, Tilemap map, TileBase tilebase);
    }
}
