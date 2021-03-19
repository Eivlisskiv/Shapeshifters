using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.OOP.TileMaps
{
    public class CaveRoom : MapRoom
    {
        int[] ceilling;
        int[] floor;

        Vector2 perlinOffset;

        float density;

        public CaveRoom(Vector2Int start, Vector2Int size) : base (start, size) { }

        public override void Initialize()
        {
            density = LoadDensity(0.3f);

            perlinOffset = new Vector2(Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f));

            int s = size.x / segment;
            int m = (size.y / 6) + borderWidth;
            ceilling = GetBorder(s, end.y, m, -1);
            floor = GetBorder(s, 0, m, 1);
            Close();
        }


        float LoadDensity(float last)
            => Mathf.Clamp(last +
                Random.Range(-0.1f, 0.1f),
                0.25f, 0.588f);

        int[] GetBorder(int s, int f, int m, int d)
        {
            int[] border = new int[s + 1];
            for(int i = 0; i < border.Length; i++)
                border[i] = f + (Random.Range(borderWidth, m) * d);
            border[s] = border[s - 1];
            return border;
        }

        private void Close()
        {
            int l = ceilling.Length - 1;
            ceilling[l] = start.y;
            ceilling[l - 1] = start.y;
            floor[l] = end.y;
            floor[l - 1] = end.y;
        }

        public override bool DrawOne(Tilemap map, TileBase tilebase)
        {
            Vector2Int border = CurrentBorder();
            var tile = GetTile(new Vector2Int(current.x, current.y), border);
            mapContent[(current.x - start.x), (current.y - start.y)] = tile;
            map.SetTile(new Vector3Int(current.x, current.y, 0), 
                tile == MapTileType.Wall ? tilebase : null);

            return Next(border);
        }

        public override bool DrawNext(Tilemap map, TileBase tilebase)
        {
            bool empty, next;
            do
            {
                Vector2Int border = CurrentBorder();
                var tile = GetTile(new Vector2Int(current.x, current.y), border);
                mapContent[(current.x - start.x), (current.y - start.y)] = tile;
                if (tile != MapTileType.Wall) empty = true;
                else
                {
                    map.SetTile(new Vector3Int(current.x, current.y, 0),
                        tile == MapTileType.Wall ? tilebase : null);
                    empty = false;
                }

                next = Next(border);

            } while (next && empty);

            return next;
        }

        public override bool DrawAmount(int amount, Tilemap map, TileBase tilebase)
        {
            bool next;
            do
            {
                Vector2Int border = CurrentBorder();
                var tile = GetTile(new Vector2Int(current.x, current.y), border);
                mapContent[(current.x - start.x), (current.y - start.y)] = tile;
                if (tile == MapTileType.Wall)
                {
                    map.SetTile(new Vector3Int(current.x, current.y, 0),
                        tile == MapTileType.Wall ? tilebase : null);
                }

                next = Next(border);
                amount--;

            } while (next && amount > 0);

            return next;
        }

        private Vector2Int CurrentBorder()
        {
            int relative = (current.x - start.x);
            int pastMark = (relative / segment);
            int travelled = relative - (pastMark * segment);

            int x = GetBorderPoint(previousBorder.x, ceilling[pastMark], travelled);
            int y = GetBorderPoint(previousBorder.y, floor[pastMark], travelled);
            return new Vector2Int(x, y);
        }

        private int GetBorderPoint(int prev, int next, int travelled)
        {
            Vector2 vect = new Vector2(segment, next) - new Vector2(0, prev);
            float part = (travelled / (float)segment);
            vect *= part;
            return Mathf.RoundToInt(prev + vect.y);
        }


        private MapTileType GetTile(Vector2Int v, Vector2Int border)
        {
            if (v.y <= border.y || v.y >= border.x) return MapTileType.Wall;
            else if (v.y <= border.y + spacing || v.y >= border.x - spacing) 
                return MapTileType.Empty;

            float p = Mathf.PerlinNoise(
                (v.x / (size.x * density)) + perlinOffset.x,
                (v.y / (size.y * density)) + perlinOffset.y);

            return p > (0.4 + (density / 2.5)) && SideSpace(v) ? MapTileType.Wall 
                : MapTileType.Empty;
        }

        private bool SideSpace(Vector2Int v)
            => !(v.x >= size.x - (spacing + borderWidth * 2)) &&
                (v.x > spacing + (borderWidth * 2));

        public bool Next(Vector2Int border)
        {
            if (current.y == end.y)
            {
                if (current.x == end.x) return false;

                current.y = start.y;
                current.x++;
                if (current.x % segment == 0)
                {
                    //density = LoadDensity(density);
                    previousBorder = border; 
                }
            }
            else current.y++;

            return true;
        }
    }
}
