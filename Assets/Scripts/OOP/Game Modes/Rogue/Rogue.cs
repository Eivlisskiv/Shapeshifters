﻿using Scripts.OOP.TileMaps;
using Scripts.OOP.UI;
using Scripts.OOP.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.Game_Modes.Rogue
{
    public class Rogue : AGameMode, IRogueMenu
    {
        public int points;
        private enum Stage { Waves, Pausing, Menu, Resuming, PassGate }

        private ObjectiveElement objective;
        private Text progress;
        private bool spawns;

        private float cooldown;
        private int level;

        private int SpawnsLeft
        {
            get => spawnsLeft;
            set
            {
                spawnsLeft = value;
                UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            if (progress) progress.text = (GetTeam(1).Count + spawnsLeft).ToString();
        }

        private int spawnsLeft;

        private Stage stage;

        private readonly ShopHandler shop;

        public Rogue(MainMenuHandler menu, MapHandler map)
            : base(menu, map, new Dictionary<string, (float, string[])>() {
                { "Regular", (100, new[]{ "Regular", "Bomber", "Sniper", "Tank" }) }
            }, Color.green, Color.red)
        {
            cooldown = 5;
            level = 1;
            SpawnsLeft = 5;

            GameObject shopPrefab = Resources.Load<GameObject>(RessourcePath + "Shop");
            shop = Object.Instantiate(shopPrefab, menu.transform.parent).GetComponent<ShopHandler>();
            shop.gameObject.SetActive(false);
        }

        public override void OnUpdate()
        {
            if (!Loaded) return;

            if (cooldown > 0)
            {
                cooldown -= Time.unscaledTime;
                return;
            }

            switch (stage)
            {
                case Stage.Pausing:
                    if (!TimeHandler.Instance.LerpScale(0, 0.02f)) return;
                    StartIntermission(); return;

                case Stage.Resuming:
                    if (!TimeHandler.Instance.LerpScale(1, 0.02f)) return;
                    ReachNext(); return;

                case Stage.PassGate:
                    if (PlayerReady()) NextWave();
                    return;
            }

            if (spawns && SpawnsLeft > 0)
                SpawnEnemy();
        }

        public override void MemberDestroyed(BaseController member)
        {
            base.MemberDestroyed(member);

            if (member is PlayerController player)
            {
                player.cam.Detach();
                return;
            }

            points += Mathf.Max(1, member.Level / 5);

            UpdateProgress();

            if (GetTeam(1).Count == 0 && SpawnsLeft == 0)
                FinishWave(); 
        }

        private void FinishWave()
        {
            score++;
            if (!map.loading.Loaded) 
                map.loading.tilesPerFrame = 50;
            stage = Stage.Pausing;
            PauseHandler.SetControl(false);
            cooldown = 5;
        }

        private void StartIntermission()
        {
            PauseControllers(true);
            stage = Stage.Menu;
            shop.gameObject.SetActive(true);
            shop.pointsDisplay.text = points.ToString();
        }

        public void MenuClosed()
        {
            stage = Stage.Resuming;

            PauseControllers(false);
            PauseHandler.SetControl(true);
        }

        private void ReachNext()
        {
            stage = Stage.PassGate;
            map.current.OpenGate(true);

            objective = Objectives.CreateObjective("Gate", Color.cyan);
            objective.Get<Text>("Title", t =>
            {
                t.text = "Reach the next room ---->";
                t.alignment = TextAnchor.MiddleCenter;
            });
        }

        private bool PlayerReady()
        {
            var players = GetTeam(0);
            float x = map.loading.CharacterPosition(new Vector2Int(3, 0)).x;
            for (int i = 0; i < players.Count; i++)
            {
                BaseController player = players[i];
                if (player.transform.position.x < x)
                    return false;
            }
            return true;
        }

        private void NextWave()
        {
            stage = Stage.Waves;

            Objectives.Remove(objective);
            objective = Objectives.Current;

            cooldown = 10;
            level++;

            SpawnsLeft = level * 5;

            map.NextRoom(MapRoom.RandomSize());
            ClearDebris();
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            map.QueuRoom(MapRoom.RandomSize());
            SpawnPlayer();
        }

        protected override void OnReady()
        {
            objective = Objectives.CreateObjective("Main", Color.red);
            objective.Get<Text>("Title").text = "Eliminate all the enemies.";
            objective.Get<Text>("Objective").text = "Enemies Left: ";

            progress = objective.Get<Text>("Progress", t =>
            {
                t.alignment = TextAnchor.MiddleCenter;
            });

            UpdateProgress();

            spawns = true;
        }

        private void SpawnPlayer()
        {
            PlayerController player = PlayerController.Instantiate
                (map.characterPrefab, map.uiPrefab,
                Camera.main, map.mainCanvas.transform);

            player.transform.position = map.current.CharacterPosition(new Vector2Int
                (MapRoom.spacing + (MapRoom.borderWidth * 2) - 1, map.width / 4));
            shop.player = player;
            AddMember(0, player);
        }

        private void SpawnEnemy()
        {
            if (CheckEnemySpawns(out Vector2Int coords))
            {
                SpawnRandom(1, coords, level);
                SpawnsLeft--;
                cooldown = 5;
            }
        }

        private bool CheckEnemySpawns(out Vector2Int pos)
        {
            pos = Vector2Int.zero;
            if (map.current == null) return false;
            if (GetTeam(1).Count < 3)
            {
                MapTileType type = map.current.RandomTile(out pos);

                return type == MapTileType.Empty;
            }

            return false;
        }
    }
}
