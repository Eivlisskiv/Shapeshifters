using Scripts.OOP.Perks;
using Scripts.OOP.TileMaps;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Game_Modes.Arena
{
    public class Arena : AGameMode, IControllerLevelUp, IElimination
    {
        private float spawnCooldown;
        private int level;

        public Arena(MainMenuHandler menu, MapHandler map) 
            : base(menu, map, new Dictionary<string, (float, string[])>() {
                { "Regular", (100, new[]{ "Regular", "Bomber", "Sniper", "Tank" }) }
            }, Color.green, Color.red)
        {
            spawnCooldown = 5;
            level = 1;
        }

        public override void OnUpdate()
        {
            if (map.current == null || !map.current.Loaded) return;

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
            }

            base.MemberDestroyed(member);
        }

        public void Elimenation(BaseController victim, BaseController killer)
        {
            if (!victim || !killer) return;

            if (victim.perks.Count > 0)
                killer.perks.Add(victim.perks.RandomDrop(), killer is PlayerController player ? player.UI : null);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            PlayerController player = PlayerController.Instantiate
                (map.characterPrefab, map.uiPrefab,
                Camera.main, map.mainCanvas.transform);

            player.transform.position = map.current.CharacterPosition(new Vector2Int
                (MapRoom.spacing + (MapRoom.borderWidth * 2) - 1, map.width / 4));

            AddMember(0, player);
        }

        private void SpawnEnemy()
        {
            if (CheckEnemySpawns(out Vector2Int coords))
                SpawnRandom(1, coords, level);
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
                level = player.Level;
                score = player.Level;

                if (player.Level == 1 || player.Level % 5 == 0)
                {
                    Perk perk = PerksHandler.Random();
                    perk.LevelUp();
                    player.perks.Add(perk, player.UI);
                }
            }
        }
    }
}
