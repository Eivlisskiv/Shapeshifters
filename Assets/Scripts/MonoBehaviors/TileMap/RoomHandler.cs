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

    public void SetSettings(int w, int h, TileBase tile, RoomHandler prev)
    {
        width = w;
        height = h;
        this.tile = tile;
        if (prev) lastEnd = new Vector2Int
                (prev.EndV.x, (height - prev.height) / 2);

    }

    // Start is called before the first frame update
    void Start()
    {
        roomContent = new GameObject("RoomContent");
        map = GetComponent<Tilemap>();
        transform.localScale = new Vector3(1, 1, 0);
        map.ClearAllTiles();

        current = new CaveRoom(lastEnd, new Vector2Int(width, height))
        { previousBorder = new Vector2Int(0, height) };
    }

    void Update()
    {
        if (!loaded && current != null) LoadRoom();
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

    public MapTileType RandomTile(out Vector2Int pos)
    {
        var content = current.mapContent;
        pos = new Vector2Int(
                Random.Range(0, content.GetLength(0)),
                Random.Range(0, content.GetLength(1)));
        return content[pos.x, pos.y];
    }

    private void OnDestroy() => Destroy(roomContent);


}
