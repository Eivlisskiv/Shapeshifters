using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.OOP.TileMaps
{
    public abstract class MapRoom
    {
        public const int spacing = 8;
        public const int borderWidth = 3;
        public const int segment = 10;

        public static int MinHeight => (borderWidth + spacing) * 3;
        public static int RandomSize()
            => Random.Range(MinHeight * 2, MinHeight * 6);

        protected Vector2Int start;
        public Vector2Int Start
        {
            get => start;
        }

        protected Vector2Int size;
        protected Vector2Int end; 
        public Vector2Int End
        {
            get => end;
        }

        protected Vector2Int current;

        public Vector2Int previousBorder;

        public MapTileType[,] mapContent;

        public Vector2[] enemySpawns;
        public Vector2 playerSpawn;

        public MapRoom(Vector2Int start, Vector2Int size)
        {
            current = start;
            this.start = start;
            this.size = size;
            end = start + size;
            mapContent = new MapTileType[size.x + 1, size.y + 1];

            Initialize();
        }

        public abstract void Initialize();
        public abstract bool DrawOne(Tilemap map, TileBase tile);
        public abstract bool DrawNext(Tilemap map, TileBase tile);
        public abstract bool DrawAmount(int amount, Tilemap map, TileBase tile);
    }
}
