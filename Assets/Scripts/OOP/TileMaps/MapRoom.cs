using Scripts.OOP.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.OOP.TileMaps
{
    public abstract class MapRoom
    {
        protected Vector2Int start;
        public Vector2Int Start
        {
            get => start;
        }

        protected readonly Vector2Int size;
        public Vector2Int Size
        {
            get => size;
        }

        protected Vector2Int current;

        private readonly int[,] mapContent;

        public MapTileType GetTile(Vector2Int pos)
            => (MapTileType)mapContent[(size.y - 1) - pos.y, pos.x];

        public void SetTile(Vector2Int pos, MapTileType tile)
            => mapContent[(size.y - 1) - pos.y, pos.x] = (int)tile;

        protected List<Vector2Int> gates = new List<Vector2Int>();
        protected List<Vector2Int> spawns = new List<Vector2Int>();

        protected bool entrance;

        private bool isGateOpen;

        protected readonly Transform propsContainer;

        public MapRoom(RoomHandler previous, Vector2Int size, Transform propsContainer)
        {
            this.size = size;
            mapContent = new int[size.y, size.x];
            this.propsContainer = propsContainer;

            Construct(previous);
        }

        public MapRoom(RoomHandler previous, int[,] tiles, Transform propsContainer)
        {
            mapContent = tiles;
            size = new Vector2Int(tiles.GetLength(1),
                                 tiles.GetLength(0));

            this.propsContainer = propsContainer;

            Construct(previous);
        }

        private void Construct(RoomHandler previous)
        {
            entrance = !!previous;
            current = Vector2Int.zero;

            if (!previous) start = Vector2Int.zero;
            else
            {
                var lastPosition = previous.transform.localPosition;
                start = new Vector2Int(
                    Mathf.RoundToInt(lastPosition.x + previous.Width),
                    Mathf.RoundToInt(lastPosition.y + ((previous.Height - size.y) / 2)));
            }

            Initialize();
        }

        public abstract void Initialize();

        public abstract void LoadFinished();

        public abstract bool DrawOne(Tilemap map, TileBase tilebase, bool center, out MapTileType tile);

        protected bool HandleTileDraw(Tilemap map, TileBase tilebase, MapTileType tile)
        {
            if (tile == MapTileType.Empty)
                return false;

            if (tile == MapTileType.Gate)
            {
                gates.Add(current);
                if (isGateOpen) return false;
            }

            if (tile == MapTileType.Spawn)
            {
                spawns.Add(current);
                return false;
            }

            map.SetTile(new Vector3Int(current.x, current.y, 0),
                tilebase);

            return true;
        }

        public virtual bool Next()
        {
            if (current.y < size.y - 1)
            {
                current.y++;
                return true;
            }

            if (current.x < size.x - 1)
            {
                current.y = 0;
                current.x++;
                return true;
            }

            return false;
        }

        public bool DrawNext(Tilemap map, TileBase tilebase, bool center)
        {
            bool empty, next;
            do
            {
                next = DrawOne(map, tilebase, center, out MapTileType tile);
                empty = tile == MapTileType.Empty;

            } while (next && empty);

            return next;
        }

        public bool DrawAmount(int amount, Tilemap map, TileBase tile, bool center)
        {
            bool next;
            do
            {
                next = DrawOne(map, tile, center, out _);
                amount--;

            } while (next && amount > 0);

            return next;
        }

        public Vector2Int OpenGate(bool open, Tilemap map, TileBase tilebase)
        {
            Vector2Int c = Vector2Int.zero;

            isGateOpen = open;

            for (int i = 0; i < gates.Count; i++)
            {
                c = gates[i];
                map.SetTile(new Vector3Int(c.x, c.y, 0), open ? null : tilebase);
            }

            return c;
        }

        public Vector2Int RandomSpawn() => spawns.RandomElement();
    }
}
