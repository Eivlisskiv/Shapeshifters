using Scripts.OOP.TileMaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomHandler : MonoBehaviour
{
    TileBase tile;

    int width;
    int height;

    Tilemap map;

    Vector2Int lastEnd = Vector2Int.zero;

    GameObject roomContent;
    MapRoom current;

    bool loaded = false;
    public bool Loaded
    { get => loaded; }

    public Vector2Int StartV => current.Start;
    public Vector2Int EndV => current.End;

    public void SetSize(int w, int h)
    {
        width = w;
        height = h;
    }

    public void SetPrevious(RoomHandler prev)
    {
        if (prev)
        {
            lastEnd = new Vector2Int(prev.EndV.x, (height - prev.height) / 2);

            //Set Door
        }
    }

    public void SetTile(TileBase tile) => this.tile = tile;

    // Start is called before the first frame update
    void Start()
    {
        roomContent = new GameObject("RoomContent");
        map = GetComponent<Tilemap>();
        transform.localScale = new Vector3(1, 1, 0);
        map.ClearAllTiles();
        //SetForeground();
    }

    //void SetForeground()
    //{
    //    if (!mask) return;

    //    Vector2 pixSize = new Vector2(width, height) / mask.transform.localScale;
    //    if (pixSize.x > 100)// is bigger than 100x50
    //    {
    //        int a = Mathf.CeilToInt(pixSize.x / 100);
    //        if (a % 2 != 0) a++;
    //        float l = Mathf.Log(a, 2f);
    //        pixSize /= l;
    //        Vector2 pos = new Vector2(width, height) / a;
    //        for (int x = 0; x < l; x++)
    //        {
    //            for (int y = 0; y < l; y++)
    //            {
    //                var m = (x + 1 == l && y + 1 == l) ? mask : Instantiate(mask, transform);
    //                m.size = pixSize;
    //                m.transform.localPosition = (pos + (pos * 2 * new Vector2(x, y))) + lastEnd;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        mask.size = new Vector2(100, 50);
    //        mask.transform.localPosition = (new Vector2(width, height) / 2) + lastEnd;
    //    }
    //}

    // Update is called once per frame

    void Update()
    {
        if (!loaded)
        {
            if (current != null) LoadRoom();
            else StartRoom(); //First room
        }
    }

    private void LoadRoom()
    {
        if (!current.DrawAmount(10, map, tile))
        {
            //After the map is loaded, load the collider. Thus doind this only once.
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            if (collider) collider.enabled = true;

            loaded = true;
        }
    }

    public WaveData StartWave(int level)
        => new WaveData(current.mapContent, roomContent.transform, level);

    private void StartRoom()
    {
        current = new CaveRoom(lastEnd, new Vector2Int(width, height))
        { previousBorder = new Vector2Int(0, height) };
    }

    private void OnDestroy() => Destroy(roomContent);
}
