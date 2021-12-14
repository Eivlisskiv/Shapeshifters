using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.OOP.TileMaps.Procedural
{
    public class CaveRoom : ProceduralMapRoom
    {
        int[] ceilling;
        int[] floor;

        Vector2 perlinOffset;

        float density;
        (Vector2, int)[] clusters;

        public override int TileBaseIndex => 0;

        public CaveRoom(Vector2Int size, RoomHandler previous,
            Transform propsContainer)
            : base(size, previous, propsContainer)
        { }

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
            int mid = size.y / 2;
            const int offset = 2;
            ceilling[l - 1] = mid - offset;
            ceilling[l] = ceilling[l - 1];
            floor[l - 1] = mid + offset;
            floor[l] = floor[l - 1];
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

        public override bool DrawOne(Tilemap map, TileBase tilebase, bool center, out MapTileType tile)
        {
            Vector2Int border = CurrentBorder();
            tile = GetTile(current, border, center);
            if (tile != MapTileType.Empty && IsGate(current))
                tile = MapTileType.Gate;

            SetTile(current, tile);

            HandleTileDraw(map, tilebase, tile);

            return Next(border);
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


        private MapTileType GetTile(Vector2Int v, Vector2Int border, bool hasCenter)
        {
            //Leave empty space for entrance
            if (IsEntrance(v)) return MapTileType.Empty;
            //Assure   floor    and       ceilling
            else if (v.y <= border.y || v.y >= border.x) return MapTileType.Wall;
            //Assure space between floor/ceilling and middle
            else if (v.y <= border.y + spacing || v.y >= border.x - spacing)
                return MapTileType.Empty;
            //Assure left/right space
            else if (SideSpace(v.x)) return MapTileType.Empty;

            return hasCenter && PerlinWall(v)
                ? MapTileType.Wall : MapTileType.Empty;
        }

        private bool IsEntrance(Vector2Int v)
        {
            if (!entrance) return false;
            int midY = size.y / 2;
            if (v.y > midY + 3 || v.y < midY - 3) return false;
            return  v.x < spacing;
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

        private bool IsGate(Vector2 v)
        {
            int midY = size.y / 2;
            if (v.y > midY + 3 || v.y < midY - 3) return false;
            return v.x > size.x - spacing;
        }

        private bool SideSpace(int x)
        {
            int margin = spacing + (borderWidth * 2);
            return x < margin || x > size.x - margin;
        }
                

        public bool Next(Vector2Int border)
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
                if (current.x % segment == 0)
                    previousBorder = border;
                return true;
            }

            return false;
        }
    }
}
