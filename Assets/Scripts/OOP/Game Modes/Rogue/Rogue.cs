using Scripts.OOP.GameModes;
using Scripts.OOP.TileMaps;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Assets.Scripts.OOP.Game_Modes.Rogue
{
    public class Rogue : AGameMode, IRogueMenu
    {
        private enum Stage { Waves, Pausing, Menu, Resuming, PassGate }

        private float cooldown;
        private int level;
        private int spawnsLeft;

        private Stage stage;

        public Rogue(MainMenuHandler menu, MapHandler map)
            : base(menu, map, Color.green, Color.red)
        {
            cooldown = 5;
            level = 1;
            spawnsLeft = 3;
        }

        public override void OnUpdate()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                return;
            }

            switch (stage)
            {
                case Stage.Pausing:
                    if (!TimeHandler.Instance.LerpScale(0, Time.deltaTime * 5)) return;
                    StartIntermission(); return;

                case Stage.Resuming:
                    if (!TimeHandler.Instance.LerpScale(1, Time.deltaTime * 5)) return;
                    ReachNext(); return;

                case Stage.PassGate:
                    //Wait for player to be in next area
                    //Close exit
                    if(PlayerReady()) FinishIntermission();
                    return;
            }

            if (spawnsLeft > 0)
                SpawnEnemy();
        }

        public override void MemberDestroyed(BaseController member)
        {
            if (member is PlayerController player)
            {
                player.cam.Detach();
            }
            else
            {
                score += member.Level;
                if (GetTeam(1).Count == 0 && spawnsLeft == 0)
                    FinishWave();
            }

            base.MemberDestroyed(member);
        }

        private void FinishWave()
        {
            //Open Gate
            if (!map.loading.Loaded) 
                map.loading.tilesPerFrame = 50;
            stage = Stage.Pausing;
            cooldown = 5;
        }

        private void StartIntermission()
        {
            int teamCount = TeamCount;
            for(int i = 0; i < teamCount; i++)
            {
                var team = GetTeam(i);
                for (int i1 = 0; i1 < team.Count; i1++)
                {
                    BaseController m = team[i1];
                    m.DisableController(true);
                }
            }

            stage = Stage.Menu;
            //Open menu

            //Close previous entrance if any

            //Delete older room

            //open menu
        }

        public void MenuClosed() 
            => stage = Stage.Resuming;

        private void ReachNext()
        {
            stage = Stage.PassGate;

            //Tell the player to go through gate
        }

        private bool PlayerReady()
        {
            var players = GetTeam(0);
            float y = map.loading.StartV.y + 20;
            for (int i = 0; i < players.Count; i++)
            {
                BaseController player = players[i];
                if (player.transform.position.y > y)
                    return false;
            }
            return true;
        }

        private void FinishIntermission()
        {
            //Close previous exit;
            NextWave();
        }

        private void NextWave()
        {
            stage = Stage.Resuming;
            cooldown = 10;
            level++;
            spawnsLeft = level * 5;
            map.NextRoom(MapRoom.RandomSize());
            ClearDebris();
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            map.QueuRoom(MapRoom.RandomSize());
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
            {
                GameObject obj = Object.Instantiate(map.characterPrefab);
                AIController mob = AIController.Spawn(obj, $"Enemy", level);
                mob.transform.position = map.current.CharacterPosition(coords);
                AddMember(1, mob);

                spawnsLeft--;
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
