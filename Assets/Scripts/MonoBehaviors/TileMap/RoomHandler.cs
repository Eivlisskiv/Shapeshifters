using Scripts.OOP.TileMaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomHandler : MonoBehaviour
{
    public Vector2Int StartV => current.Start;
    public bool hasCenter = true;
    int tilesPerFrame = 5;

    TileBase tile;

    int width;
    int height;

    Tilemap map;

    Vector2Int lastEnd = Vector2Int.zero;

    MapRoom current;

    public bool Loaded
    { get => loaded; }
    bool loaded = false;

    private bool entrance;

    public void SetSettings(int w, int h, TileBase tile, RoomHandler prev)
    {
        width = w;
        height = h;
        this.tile = tile;
        entrance = prev;
        if (prev)
        {
            var lastSize = prev.current.Size;
            var lastPosition = prev.transform.localPosition;
            lastEnd = new Vector2Int(
                Mathf.RoundToInt(lastPosition.x + lastSize.x),
                Mathf.RoundToInt(lastPosition.y + ((prev.height - height) / 2)));
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        transform.localScale = new Vector3(1, 1, 0);
        map.ClearAllTiles();

        current = new CaveRoom(lastEnd, new Vector2Int(width, height), entrance);
        map.transform.localPosition = new Vector3(current.Start.x, current.Start.y, 0);
    }

    void Update()
    {
        if (!loaded && current != null) LoadRoom();
    }

    private void LoadRoom()
    {
        if (!current.DrawAmount(tilesPerFrame, map, tile, hasCenter))
        {
            //After the map is loaded, load the collider. Thus doing this only once.
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            if (collider) collider.enabled = true;

            loaded = true;
        }
    }

    public void SetTilesPerFrame(int tpf)
        => tilesPerFrame = tpf;

    public MapTileType GetTile(Vector2Int pos)
        => current.mapContent[pos.x, pos.y];

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

    public void OpenGate(bool v) => current.OpenGate(v, map, tile);
}
