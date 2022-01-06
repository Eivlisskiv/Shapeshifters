using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using Scripts.OOP.TileMaps;
using Scripts.OOP.TileMaps.Procedural;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapHandler : MonoBehaviour
{
    public Transform inGameUI;

    public GameObject characterPrefab;
    public GameObject uiPrefab;
    public GameObject roomPrefab;

    public TileBase[] tilesets;

    //public int width;
    public bool spawnMobs;

    readonly List<RoomHandler> rooms = new List<RoomHandler>();

    public RoomHandler Previous { get; private set; }
    public RoomHandler Current 
    { 
        get => _current; 
        private set
        {
            if (_current == value) return;
            _current = value;
            if (_current != null) _current.OnCurrent();
        }
    }
    private RoomHandler _current;

    public RoomHandler Loading { get; private set; }

    public void StartMap()
    {
        if (enabled)
        {
            Debug.LogWarning("MapHandler already started");
            return;
        }
        enabled = true;
        AlignCamera(0.68f * Loading.Width);
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
        if(Loading != null)
        {
            Debug.LogWarning($"There is already a queud room");
            return;
        }

        CreateRoom((room) => 
        {
            var r = ConstructRoom<T>(new Vector2Int(size, size / 2),
                Current, room.propsContainer);

            room.SetSettings(r, tilesets[Math.Min
            (r.TileBaseIndex, tilesets.Length - 1)], 0);
        });
    }

    public void QueuRoom(MapPreset preset)
    {
        if (Loading != null)
        {
            Debug.LogWarning($"There is already a queud room");
            return;
        }

        CreateRoom((room) =>
        {
            FixedMapRoom r = new FixedMapRoom(preset, Current, room.propsContainer);
            room.SetSettings(r, tilesets[Math.Min
                (r.tileBaseIndex, tilesets.Length - 1)], preset.yOffset);
        });
    }

    private T ConstructRoom<T>(params object[] args)
        where T : MapRoom
        => (T) System.Activator.CreateInstance(typeof(T), args);

    public void NextRoom(MapPreset preset, bool isRemoveOld)
    {
        if (Loading != null)
        {
            if (!Loading.Loaded) Loading.SetTilesPerFrame(999);

            if (Previous && isRemoveOld) Destroy(Previous.gameObject);

            Previous = Current;

            Current = Loading;

            Loading = null;
        }

        if(preset != null) QueuRoom(preset);
    }

    public void NextProceduralRoom(int size)
    {
        if(Loading != null && !Loading.Loaded)
        {
            Debug.LogWarning($"Loading room is still loading");
            return;
        }

        if (Previous) Destroy(Previous.gameObject);
        Previous = Current;

        Previous.OpenGate(false);

        Current = Loading;

        Loading = null;

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
        Loading = room;
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
        if (Loading != null && Loading.Loaded)
        {
            //Asign as first room
            if (Current == null)
                FirstLoad();
        }
    }

    private void FirstLoad()
    {
        Current = Loading;
        Loading = null;
        GameModes.GameMode.OnLoaded();
    }

    public void Clear()
    {
        Loading = null;
        Current = null;

        int c = transform.childCount;
        for (int i = 0; i < c; i++)
            Destroy(transform.GetChild(i).gameObject);

        GameModes.GameMode?.Clear();

        enabled = false;
    }
}
