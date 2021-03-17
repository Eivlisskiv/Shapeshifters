using Scripts.OOP.TileMaps;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapHandler : MonoBehaviour
{
    public Canvas mainCanvas;

    public GameObject characterPrefab;
    public GameObject uiPrefab;
    public GameObject roomPrefab;

    public TileBase[] tilesets;

    public int width;

    MainMenuHandler menu;

    List<RoomHandler> rooms;

    RoomHandler current;
    RoomHandler loading;

    WaveData Wave => WaveData.Wave;

    public void StartMap(MainMenuHandler menu)
    {
        if (menu) this.menu = menu;
        rooms = new List<RoomHandler>();
        int height = width / 2;
        GenerateRoom(width, height, null);
        float size = 0.70f * width;
        Camera.main.orthographicSize = size;
        Camera.main.transform.position = new Vector3((size * 2.2f), size * 1.08f, -10);
    }

    private void GenerateRoom(int width, int height, RoomHandler previous)
    {
        GameObject robj = Instantiate(roomPrefab);
        robj.transform.parent = transform;
        robj.name = $"Room{rooms.Count}";
        RoomHandler room = robj.GetComponent<RoomHandler>();
        room.SetSize(width, height);
        room.SetPrevious(previous);
        room.SetTile(tilesets[0]);
        rooms.Add(room);
        loading = room;
    }

    // Update is called once per frame
    void Update()
    {
        VerifyLoadingRoom();

        if (Wave != null) HandleWave();
    }

    private void VerifyLoadingRoom()
    {
        //If room is done loading
        if (loading != null && loading.Loaded)
        {
            //Asign as first room
            if (current == null)
            {
                NextRoom();
                SpawnPlayer();
                //GenerateRoom(width, width / 2, current);

                menu.SetStartButton(true);
                menu.container.SetActive(false);
            }
        }
    }

    private void NextRoom()
    {
        current = loading;
        loading = null;

        //Next wave
        current.StartWave((Wave?.level ?? 0) + 1);
    }

    public void SpawnPlayer()
    {
        PlayerController.Spawn(characterPrefab, uiPrefab, 
            Camera.main, mainCanvas.transform, CharacterPosition(
            new Vector2Int(MapRoom.spacing + (MapRoom.borderWidth * 2), width / 4)));
    }

    public void HandleWave()
    {
        if (Wave.CheckEnemySpawns(out Vector2Int coords))
        {
            AIController mob = Wave.SpawnMob(Instantiate(characterPrefab));
            mob.transform.position = CharacterPosition(coords + current.StartV);
        }
    }

    private Vector2 CharacterPosition(Vector2Int coords)
    => new Vector3((coords.x + transform.position.x) * transform.localScale.x,
        (coords.y + transform.position.y) * transform.localScale.y);

    public void Clear()
    {
        loading = null;
        current = null;


        int c = transform.childCount;
        for (int i = 0; i < c; i++)
            Destroy(transform.GetChild(i).gameObject);

        if (Wave != null) Wave.Clear();

        enabled = false;
    }
}
