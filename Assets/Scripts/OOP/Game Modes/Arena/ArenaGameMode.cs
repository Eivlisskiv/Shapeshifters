using Scripts.OOP.TileMaps;
using UnityEngine;

namespace Scripts.OOP.GameModes.Arena
{
    public class ArenaGameMode : AGameMode, IControllerLevelUp
    {
        private float spawnCooldown;
        private int level;

        public ArenaGameMode(MainMenuHandler menu, MapHandler map) 
            : base(menu, map, Color.green, Color.red)
        {
            spawnCooldown = 5;
            level = 1;
        }

        public override void OnUpdate()
        {
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
            else
            {
                score += member.Level;
            }

            base.MemberDestroyed(member);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            //map.QueuRoom(Random.Range(40, 161));
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            PlayerController player = PlayerController.Instantiate
                (map.characterPrefab, map.uiPrefab,
                Camera.main, map.mainCanvas.transform);

            player.transform.position = map.CharacterPosition(new Vector2Int
                (MapRoom.spacing + (MapRoom.borderWidth * 2), map.width / 4));

            AddMember(0, player);
        }

        private void SpawnEnemy()
        {
            if (CheckEnemySpawns(out Vector2Int coords))
            {
                GameObject obj = Object.Instantiate(map.characterPrefab);
                AIController mob = AIController.Spawn(obj, $"Enemy", level);
                mob.transform.position = map.CharacterPosition(coords);
                AddMember(1, mob);
            }
        }

        private bool CheckEnemySpawns(out Vector2Int pos)
        {
            pos = Vector2Int.zero;

            if (GetTeam(1).Count < 3)
            {
                MapTileType type = map.RandomTile(out pos);

                if (type == MapTileType.Empty)
                    return true;
            }

            return false;
        }

        public void ControllerLevelUp(BaseController controller)
        {
            if(controller is PlayerController)
                level = controller.Level;
        }
    }
}
