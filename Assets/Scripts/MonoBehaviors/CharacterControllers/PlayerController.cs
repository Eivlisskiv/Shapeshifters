﻿using Scripts.OOP.Character.Stats;
using Scripts.OOP.Perks;
using Scripts.OOP.TileMaps;
using Scripts.OOP.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    public static List<PlayerController> players = new List<PlayerController>();
    public static PlayerController Spawn(GameObject bodyPrefab,
        GameObject uiPrefab, Camera cam, Transform uiCanvas, Vector3 position)
    {
        GameObject body = Instantiate(bodyPrefab);
        body.name = "Player";
        GameObject ui = Instantiate(uiPrefab, uiCanvas);
        ui.name = $"{body.name}_UI";

        PlayerController player = body.AddComponent<PlayerController>();
        player.cam = new PlayerCamera(cam, 15f);

        player.transform.position = position;
        player.Color = Color.green;

        player.ui = ui.GetComponent<CharacterUIHandler>();
        var uit = player.ui.GetComponent<RectTransform>();
        uit.anchoredPosition = Vector3.zero;
        uit.localScale = new Vector3(1, 1, 1);

        players.Add(player);

        return player;
    }

    public PlayerCamera cam;

    private CharacterUIHandler ui;
    public CharacterUIHandler UI { get => ui; }

    public override void OnStart() 
    {
        stats = new Stats(200);
        body.corners = Random.Range(3, 10);
        if (ui)
        {
            ui.UpdateHealth(stats.HPP);
            ui.UpdateXP(xp / XPRequired);
        }
    }

    public override void OnUpdate()
    {
        cam.Update(transform.position);
    }

    public override bool IsFiring(out float angle)
    {
        if(!Input.GetMouseButton(0))
        {
            angle = 0;
            return false;
        }

        //Get the world mouse position
        Vector3 mousePos = cam.MouseToWorld();

        //Get the angle relative to the position
        Vector2 pos = transform.position;
        int y = pos.y > mousePos.y ? -1 : 1;
        angle = Vector2.Angle(Vector2.left, pos - (Vector2)mousePos) * y;

        return true;
    }

    public override void OnHealthChange() 
    {
        if(ui) ui.UpdateHealth(stats.HPP);
    }

    public override void OnXPChange(bool isUp)
    {
        if (ui)
        {
            if (isUp)
            {
                WaveData.Wave.level++;
                ui.LevelUp(level);
                if (level == 1 || level % 5 == 0)
                {
                    Perk perk = PerksHandler.Random();
                    perk.LevelUp();
                    perks.Add(perk, ui);
                }
            }
            ui.UpdateXP(xp / XPRequired);
        }
    }

    public override bool OnDeath()
    {
        players.Remove(this);
        cam.Detach();
        return true;
    }

    public override void OnDeathEnded()
    {
        if (ui) Destroy(ui.gameObject);
        MainMenuHandler.GameOver(level);
    }
}
