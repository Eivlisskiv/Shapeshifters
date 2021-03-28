using System.Collections.Generic;
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
        List<Vector2Int> gates = new List<Vector2Int>();
        (Vector2, int)[] clusters;

        public CaveRoom(Vector2Int start, Vector2Int size, bool entrance)
            : base (start, size, entrance) { }

        public override void Initialize()
        {
            density = LoadDensity(0.3f);
            float maxPerlin = 1f - density;

            perlinOffset = new Vector2(
                Random.Range(0, maxPerlin),
                Random.Range(0, maxPerlin));

            int s = (size.x / segment) + 1;
            int m = (size.y / 6) + borderWidth;
            ceilling = GetBorder(s, size.y, m, -1);
            floor = GetBorder(s, 0, m, 1);
            Close();

            GenerateClusters();
        }


        float LoadDensity(float last)
            => Mathf.Clamp(last +
                Random.Range(-0.1f, 0.1f),
                0.25f, 0.588f);

        int[] GetBorder(int s, int f, int m, int d)
        {
            int[] border = new int[s];
            for(int i = 0; i < border.Length; i++)
                border[i] = f + (Random.Range(borderWidth, m) * d);
            return border;
        }

        private void Close()
        {
            int l = ceilling.Length - 1;
            ceilling[l] = start.y;
            ceilling[l - 1] = start.y;
            floor[l] = size.y;
            floor[l - 1] = size.y;
        }

        private void GenerateClusters()
        {
            clusters = new (Vector2, int)[Random.Range(4, 9)];
            int ax = size.x / 2;
            int ix = size.x / 4;
            int ay = size.y / 2;
            int iy = size.y / 4;
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i] = (new Vector2(
                    Random.Range(ix / 2, ax),
                    Random.Range(iy / 2, ay)),
                    Random.Range(iy, ix));
            }
        }

        public override bool DrawOne(Tilemap map, TileBase tilebase, out MapTileType tile)
        {
            Vector2Int border = CurrentBorder();
            tile = GetTile(new Vector2Int(current.x, current.y), border);
            HandleTileDraw(map, tilebase, tile);

            return Next(border);
        }

        public override bool DrawNext(Tilemap map, TileBase tilebase)
        {
            bool empty, next;
            do
            {
                next = DrawOne(map, tilebase, out MapTileType tile);
                empty = tile == MapTileType.Empty;

            } while (next && empty);

            return next;
        }

        public override bool DrawAmount(int amount, Tilemap map, TileBase tilebase)
        {
            bool next;
            do
            {
                next = DrawOne(map, tilebase, out _);
                amount--;

            } while (next && amount > 0);

            return next;
        }

        private bool HandleTileDraw(Tilemap map, TileBase tilebase, MapTileType tile)
        {
            mapContent[current.x, current.y] = tile;
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

        private Vector2Int CurrentBorder()
        {
            int relative = current.x;
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
            //Leave empty space for entrance
            if (IsGate(v, out MapTileType r)) return r;
            //Assure   floor    and       ceilling
            else if (v.y <= border.y || v.y >= border.x) return MapTileType.Wall;
            //Assure space between floor/ceilling and middle
            else if (v.y <= border.y + spacing || v.y >= border.x - spacing)
                return MapTileType.Empty;
            //Assure left/right space
            else if (SideSpace(v.x)) return MapTileType.Empty;

            if (PerlinWall(v)) return MapTileType.Wall;

            return MapTileType.Empty;
        }

        private bool ClusterWall(Vector2Int v)
        {
            for (int i = 0; i < clusters.Length; i++)
            {
                (Vector2 core, int size) = clusters[i];
                Vector2 dist = core - v;
                if (dist.magnitude < size) return true;
            }
            return false;
        }

        private bool PerlinWall(Vector2Int v)
        {
            Vector2 perlinPos = perlinOffset + (new Vector2(v.x / (float)size.x,
                v.y / (float)size.y) / density);

            float p = Mathf.PerlinNoise(perlinPos.x, perlinPos.y);

            return p > (0.3 + (density / 2.5));
        }

        private bool IsGate(Vector2 v, out MapTileType tile)
        {
            tile = MapTileType.Empty;
            int midY = size.y / 2;
            if (v.y > midY + 3 || v.y < midY - 3) return false;

            if (entrance && v.x < spacing) return true;

            tile = MapTileType.Gate;
            return v.x > size.x - spacing;
        }

        private bool SideSpace(int x)
        {
            int margin = spacing + (borderWidth * 2);
            return x < margin || x > size.x - margin;
        }
                

        public bool Next(Vector2Int border)
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
                if (current.x % segment == 0)
                    previousBorder = border;
                return true;
            }

            return false;
        }

        public override void OpenGate(bool open, Tilemap map, TileBase tilebase)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                Vector2Int c = gates[i];
                map.SetTile(new Vector3Int(c.x, c.y, 0), open ? null : tilebase);
            }
        }
    }
}
