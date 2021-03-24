﻿using Scripts.OOP.GameModes;
using Scripts.OOP.TileMaps;
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

    public int width;
    public bool spawnMobs;

    List<RoomHandler> rooms;

    RoomHandler current;
    RoomHandler loading;

    public void StartMap()
    {
        rooms = new List<RoomHandler>();
        int height = width / 2;
        GenerateRoom(width, height, null);
        AlignCamera(0.68f * width);
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

    public void QueuRoom(int size)
    {
        if(loading != null)
        {
            Debug.LogWarning($"There is already a queud room");
            return;
        }

        GenerateRoom(size, size / 2, current);
    }

    private void GenerateRoom(int width, int height, RoomHandler previous)
    {
        GameObject robj = Instantiate(roomPrefab);
        robj.transform.parent = transform;
        robj.name = $"Room{rooms.Count}";
        RoomHandler room = robj.GetComponent<RoomHandler>();
        room.SetSettings(width, height, tilesets[0], previous);
        rooms.Add(room);
        loading = room;
    }

    // Update is called once per frame
    void Update()
    {
        VerifyLoadingRoom();

        AGameMode.GameMode?.OnUpdate();
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
        AGameMode.GameMode.OnLoaded();
    }

    public Vector2 CharacterPosition(Vector2Int coords)
    => new Vector3((coords.x + transform.position.x) * transform.localScale.x,
        (coords.y + transform.position.y) * transform.localScale.y);

    public MapTileType RandomTile(out Vector2Int pos)
        => current.RandomTile(out pos);

    public void Clear()
    {
        loading = null;
        current = null;

        int c = transform.childCount;
        for (int i = 0; i < c; i++)
            Destroy(transform.GetChild(i).gameObject);

        AGameMode.GameMode?.Clear();

        enabled = false;
    }
}
