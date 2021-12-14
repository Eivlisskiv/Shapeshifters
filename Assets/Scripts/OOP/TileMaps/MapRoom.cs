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

        protected Vector2Int size;
        public Vector2Int Size
        {
            get => size;
        }

        protected Vector2Int current;

        public MapTileType[,] mapContent;
        protected List<Vector2Int> gates = new List<Vector2Int>();

        protected bool entrance;

        protected readonly Transform propsContainer;

        public MapRoom(RoomHandler previous, Vector2Int size, Transform propsContainer)
        {
            this.propsContainer = propsContainer;
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

            this.size = size;
            Initialize();
        }

        public abstract void Initialize();

        public abstract void LoadFinished();

        public abstract bool DrawOne(Tilemap map, TileBase tilebase, bool center, out MapTileType tile);

        protected bool HandleTileDraw(Tilemap map, TileBase tilebase, MapTileType tile)
        {
            if (tile != MapTileType.Empty)
            {
                if (tile == MapTileType.Gate)
                    gates.Add(current);

                map.SetTile(new Vector3Int(current.x, current.y, 0),
                    tile == MapTileType.Wall || tile == MapTileType.Gate
                    ? tilebase : null);

                return false;
            }

            return true;
        }

        public virtual bool Next()
        {
            if (current.y < size.y)
            {
                current.y++;
                return true;
            }

            if (current.x < size.x)
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

        public void OpenGate(bool open, Tilemap map, TileBase tilebase)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                Vector2Int c = gates[i];
                map.SetTile(new Vector3Int(c.x, c.y, 0), open ? null : tilebase);
            }
        }
    }
}
