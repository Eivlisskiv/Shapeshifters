using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.Story;
using Scripts.OOP.TileMaps;
using Scripts.OOP.TileMaps.Procedural;
using System;
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

    //public int width;
    public bool spawnMobs;

    readonly List<RoomHandler> rooms = new List<RoomHandler>();

    internal RoomHandler previous;
    internal RoomHandler current;
    internal RoomHandler loading;

    public void StartMap()
    {
        if (enabled)
        {
            Debug.LogWarning("MapHandler already started");
            return;
        }
        enabled = true;
        AlignCamera(0.68f * loading.Width);
    }

    private void AlignCamera(float size)
    {
        var cam = Camera.main;
        cam.orthographicSize = size;
        cam.transform.position = new Vector3((size * 2.2f), size * 1.08f, -10);

        var background = cam.transform.GetChild(0).transform;
        float s = size / 3f;
        background.localScale = new Vector3(s, s, 1);
    }

    public void QueuRoom<T>(int size)
        where T : ProceduralMapRoom
    {
        if(loading != null)
        {
            Debug.LogWarning($"There is already a queud room");
            return;
        }

        CreateRoom((room) => 
        {
            var r = ConstructRoom<T>(new Vector2Int(size, size / 2),
                current, room.propsContainer);

            room.SetSettings(r, tilesets[Math.Min
            (r.TileBaseIndex, tilesets.Length - 1)]);
        });
    }

    public void QueuRoom(MapPreset preset)
    {
        if (loading != null)
        {
            Debug.LogWarning($"There is already a queud room");
            return;
        }

        CreateRoom((room) =>
        {
            var r = new FixedMapRoom(preset, current, room.propsContainer);
            room.SetSettings(r, tilesets[Math.Min
                (r.tileBaseIndex, tilesets.Length - 1)]);
        });
    }

    private T ConstructRoom<T>(params object[] args)
        where T : MapRoom
        => (T) System.Activator.CreateInstance(typeof(T), args);

    public void NextRoom(MapPreset preset, bool isRemoveOld)
    {
        if (loading != null)
        {
            if (!loading.Loaded) loading.SetTilesPerFrame(999);

            if (previous && isRemoveOld) Destroy(previous.gameObject);

            previous = current;

            current = loading;
            loading = null;
        }

        QueuRoom(preset);
    }

    public void NextProceduralRoom(int size)
    {
        if(loading != null && !loading.Loaded)
        {
            Debug.LogWarning($"Loading room is still loading");
            return;
        }

        if (previous) Destroy(previous.gameObject);
        previous = current;

        previous.OpenGate(false);

        current = loading;
        loading = null;

        QueuRoom<CaveRoom>(size);
    }

    private void CreateRoom(Action<RoomHandler> settings)
    {
        GameObject robj = Instantiate(roomPrefab);
        robj.transform.parent = transform;
        robj.name = $"Room{rooms.Count}";
        RoomHandler room = robj.GetComponent<RoomHandler>();

        settings(room);

        rooms.Add(room);
        loading = room;
    }

    // Update is called once per frame
    void Update()
    {
        VerifyLoadingRoom();

        GameModes.GameMode?.OnUpdate();
    }

    private void VerifyLoadingRoom()
    {
        //If room is done loading
        if (loading != null && loading.Loaded)
        {
            //Asign as first room
            if (current == null)
                FirstLoad();
        }
    }

    private void FirstLoad()
    {
        current = loading;
        loading = null;
        GameModes.GameMode.OnLoaded();
    }

    public void Clear()
    {
        loading = null;
        current = null;

        int c = transform.childCount;
        for (int i = 0; i < c; i++)
            Destroy(transform.GetChild(i).gameObject);

        GameModes.GameMode?.Clear();

        enabled = false;
    }
}
