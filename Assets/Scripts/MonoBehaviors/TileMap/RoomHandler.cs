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

    MapRoom current;

    public int tilesPerFrame = 5;

    bool loaded = false;
    public bool Loaded
    { get => loaded; }

    public Vector2Int StartV => current.Start;
    public Vector2Int EndV => current.End;

    public void SetSettings(int w, int h, TileBase tile, RoomHandler prev)
    {
        width = w;
        height = h;
        this.tile = tile;
        if (prev)
        {
            lastEnd = new Vector2Int(prev.EndV.x,
                ((prev.height - height) / 2));
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        transform.localScale = new Vector3(1, 1, 0);
        map.ClearAllTiles();

        current = new CaveRoom(lastEnd, new Vector2Int(width, height))
        { previousBorder = new Vector2Int(lastEnd.y, height) };
    }

    void Update()
    {
        if (!loaded && current != null) LoadRoom();
    }

    private void LoadRoom()
    {
        if (!current.DrawAmount(tilesPerFrame, map, tile))
        {
            //After the map is loaded, load the collider. Thus doing this only once.
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            if (collider) collider.enabled = true;

            loaded = true;
        }
    }

    public void SetTilesPerFrame(int tpf)
        => tilesPerFrame = tpf;

    public MapTileType RandomTile(out Vector2Int pos)
    {
        var content = current.mapContent;
        pos = new Vector2Int(
                Random.Range(0, content.GetLength(0)),
                Random.Range(0, content.GetLength(1)));
        return content[pos.x, pos.y];
    }

    public Vector2 CharacterPosition(Vector2Int coords)
    => new Vector3((coords.x + StartV.x) * transform.parent.transform.localScale.x,
        (coords.y + StartV.y) * transform.parent.transform.localScale.y);
}
