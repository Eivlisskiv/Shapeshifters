using Scripts.OOP.TileMaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomHandler : MonoBehaviour
{
    public BoxCollider2D bounds;

    public Vector2Int StartV => current.Start;

    public bool hasCenter = true;
    int tilesPerFrame = 5;

    TileBase tile;

    public int Width => current.Size.x;
    public int Height => current.Size.y;

    public Transform propsContainer;

    Tilemap map;

    MapRoom current;

    public bool Loaded
    { get => loaded; }
    bool loaded = false;

    public void SetSettings(MapRoom room, TileBase tile, float yOffset)
    {
        this.tile = tile;
        current = room;

        transform.localScale = new Vector3(1, 1, 0);
        transform.localPosition = new Vector3(StartV.x, StartV.y + yOffset, 0);

        if (bounds)
        {
            bounds.transform.localPosition = new Vector2(Width/2f, Height/2f);
            bounds.size = new Vector2(Width - 2, Height - 2) * 3;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        map.ClearAllTiles();
    }

    void Update()
    {
        if (!loaded && current != null) LoadRoom();
    }

    public void OnCurrent() => current.OnCurrent();

    private void LoadRoom()
    {
        if (!current.DrawAmount(tilesPerFrame, map, tile, hasCenter))
        {
            //After the map is loaded, load the collider. Thus doing this only once.
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            if (collider) collider.enabled = true;

            loaded = true;

            current.LoadFinished();
        }
    }

    public void SetTilesPerFrame(int tpf)
        => tilesPerFrame = tpf;

    public MapTileType GetTile(Vector2Int pos) => current.GetTile(pos);

    public MapTileType RandomTile(out Vector2Int pos)
    {
        pos = new Vector2Int(
                    Random.Range(0, current.Size.x),
                    Random.Range(0, current.Size.y)
                    );
        return current.GetTile(pos);
    }

    public Vector2 MapPosition(Vector2Int coords)
    {
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.parent.transform.localScale;
        return new Vector3((coords.x + pos.x) * scale.x,
                    (coords.y + pos.y) * scale.y);
    }

    public Vector2Int OpenGate(bool v) => current.OpenGate(v, map, tile);

    public Vector2Int RandomSpawn() => current.RandomSpawn();

    public bool TryGetProp(string id, out GameObject obj)
    {
        obj = null;
        return !string.IsNullOrEmpty(id) && current is FixedMapRoom fmr && fmr.TryGetProp(id, out obj);
    }
}
