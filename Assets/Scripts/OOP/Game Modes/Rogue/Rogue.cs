using Scripts.OOP.TileMaps;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.Game_Modes.Rogue
{
    public class Rogue : AGameMode, IRogueMenu
    {
        public int points;
        private enum Stage { Waves, Pausing, Menu, Resuming, PassGate }

        private float cooldown;
        private int level;
        private int spawnsLeft;

        private Stage stage;

        private readonly ShopHandler shop;

        public Rogue(MainMenuHandler menu, MapHandler map)
            : base(menu, map, Color.green, Color.red)
        {
            cooldown = 5;
            level = 1;
            spawnsLeft = 5;

            GameObject shopPrefab = Resources.Load<GameObject>(RessourcePath + "Shop");
            shop = Object.Instantiate(shopPrefab, menu.transform.parent).GetComponent<ShopHandler>();
            shop.gameObject.SetActive(false);
        }

        public override void OnUpdate()
        {
            if (!Loaded) return;

            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
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

            if (spawnsLeft > 0)
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
            if (GetTeam(1).Count == 0 && spawnsLeft == 0)
                FinishWave(); 
        }

        private void FinishWave()
        {
            score++;
            if (!map.loading.Loaded) 
                map.loading.tilesPerFrame = 50;
            stage = Stage.Pausing;
            cooldown = 5;
        }

        private void StartIntermission()
        {
            PauseControllers(true);
            stage = Stage.Menu;
            shop.gameObject.SetActive(true);
        }

        public void MenuClosed()
        {
            stage = Stage.Resuming;
            PauseControllers(false);
        }

        private void ReachNext()
        {
            stage = Stage.PassGate;
            map.current.OpenGate(true);
            //Tell the player to go through gate
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
            shop.player = player;
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
