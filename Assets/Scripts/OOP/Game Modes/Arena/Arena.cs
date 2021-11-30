using Scripts.OOP.Perks;
using Scripts.OOP.TileMaps;
using Scripts.OOP.TileMaps.Procedural;
using Scripts.OOP.UI;
using Scripts.OOP.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.Game_Modes.Arena
{
    public class Arena : ArcadeMode, IControllerLevelUp, IElimination
    {
        private bool spawns = false;
        private float spawnCooldown;
        private int level;

        private ObjectiveElement mainObjective;
        private int eliminations;
        private Text progress;

        private int switchWeapon;

        public Arena(MainMenuHandler menu, MapHandler map) 
            : base(menu, map, new Dictionary<string, (float, string[])>() {
                { "Regular/Tier1", (60, new[]{ "Regular", "Bomber", "Tank", "Sniper", }) },
                { "Regular/Tier2", (40, new[]{"Gunner", "Pirate", "Flamer" }) }
            }, Color.green, Color.red)
        {
            spawnCooldown = 5;
            level = 1;
        }

        public override void OnUpdate()
        {
            if (map.current == null || !map.current.Loaded) return;

            if (!spawns) return;

            if(spawnCooldown > 0)
            {
                spawnCooldown -= Time.deltaTime;
                return;
            }
            SpawnEnemy();
        }

        public override void MemberDestroyed(BaseController member)
        {
            if(member is PlayerController player)
            {
                player.cam.Detach();
                base.MemberDestroyed(member);
                return;
            }

            eliminations++;
            progress.text = eliminations.ToString();

            switchWeapon++;
            if (switchWeapon >= 5)
            {
                player = (PlayerController)GetTeam(0)[0];
                var weps = Weapon.Types.Where(t => t != player.Weapon.GetType()).ToArray();
                if (weps.Length > 0) player.SetWeapon(Randomf.Element(weps));

                switchWeapon = 0;
            }

            base.MemberDestroyed(member);
        }

        public void Elimenation(BaseController victim, BaseController killer)
        {
            if (!victim || !killer) return;

            if (victim.perks.Count > 0)
                killer.perks.Add(victim.perks.RandomDrop(), killer is PlayerController player ? player.UI : null);
        }

        protected override void OnMapStarted()
        {
            map.QueuRoom<CaveRoom>(80);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            SpawnPlayer();
        }

        protected override void OnReady()
        {
            mainObjective = Objectives.CreateObjective("Main", Color.red);
            mainObjective.Get<Text>("Title").text = "Eliminate enemies.";
            mainObjective.Get<Text>("Objective").text = "Eliminations: ";

            progress = mainObjective.Get<Text>("Progress", t => 
            {
                t.text = "0";
                t.alignment = TextAnchor.MiddleCenter;
            });


            spawns = true;
        }

        private void SpawnPlayer()
        {
            PlayerController player = PlayerController.Instantiate
                (map.characterPrefab, map.uiPrefab,
                Camera.main, map.mainCanvas.transform);

            player.transform.position = map.current.CharacterPosition(new Vector2Int
                (ProceduralMapRoom.spacing + (ProceduralMapRoom.borderWidth * 2) - 1, map.current.Width / 4));

            AddMember(0, player);
        }

        private void SpawnEnemy()
        {
            if (CheckEnemySpawns(out Vector2Int coords))
                SpawnEnemy(GetRandomEnemy(), 1, coords, level);
        }

        private bool CheckEnemySpawns(out Vector2Int pos)
        {
            pos = Vector2Int.zero;

            if (GetTeam(1).Count < 3)
            {
                MapTileType type = map.current.RandomTile(out pos);

                if (type == MapTileType.Empty)
                    return true;
            }

            return false;
        }

        public void ControllerLevelUp(BaseController controller)
        {
            if (controller is PlayerController player)
            {
                controller.stats.MaxHealthPoints(controller.Level);

                level = player.Level;
                score = player.Level;

                if (player.Level == 1 || player.Level % 5 == 0)
                {
                    Perk perk = PerksHandler.Random();
                    player.perks.Add(perk, player.UI);
                }
            }
        }
    }
}
