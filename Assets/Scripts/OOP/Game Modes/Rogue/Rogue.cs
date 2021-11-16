using Scripts.OOP.TileMaps;
using Scripts.OOP.UI;
using Scripts.OOP.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.Game_Modes.Rogue
{
    public class Rogue : AGameMode, IRogueMenu, IControllerLevelUp
    {
        public int points;
        private enum Stage { Waves, Pausing, Menu, Resuming, PassGate }

        private ObjectiveElement objective;
        private Text progress;
        private bool spawns;

        private float cooldown;
        private int level;

        private bool isBoss;

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

        private readonly GameObject cratePrefab;
        private readonly ShopHandler shop;
        private readonly ResourceCache<GameObject> Bosses;
        private readonly string[] bossesPaths;

        public Rogue(MainMenuHandler menu, MapHandler map)
            : base(menu, map, new Dictionary<string, (float, string[])>() {
                { "Regular/Tier1", (50, new[]{ "Regular", "Bomber", "Tank", "Sniper", }) },
                { "Regular/Tier2", (30, new[]{"Gunner", "Pirate", "Flamer" }) },
                { "Special", (20, new[]{ "Eye Holder", }) }
            }, Color.green, Color.red)
        {
            cooldown = 5;
            level = 1;
            SpawnsLeft = 5;

            GameObject shopPrefab = Resources.Load<GameObject>(RessourcePath + "Shop");
            cratePrefab = Resources.Load<GameObject>(RessourcePath + "Crate");
            shop = Object.Instantiate(shopPrefab, menu.transform.parent).GetComponent<ShopHandler>();
            shop.gameObject.SetActive(false);

            Bosses = new ResourceCache<GameObject>("Enemies/Boss/");
            bossesPaths = new string[]
            {
                "Pyramid", "Number Four"
            };
        }

        public override void OnUpdate()
        {
            if (!Loaded) return;

            if (cooldown > 0)
            {
                cooldown -= Time.unscaledDeltaTime;
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

            points += Mathf.Max(1, (member.Level / 5) * member.Body.Radius);

            UpdateProgress();

            if (GetTeam(1).Count == 0 && SpawnsLeft == 0)
            {
                if (isBoss && member is EnemyController boss) BossDrop(boss);
                FinishWave();
            }
        }

        private void FinishWave()
        {
            score++;
            if (!map.loading.Loaded) map.loading.SetTilesPerFrame(50);
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

            int height = map.loading.Height / 2;
            Vector2 gatePosition = map.loading.CharacterPosition(new Vector2Int(3, height));

            objective = Objectives.CreateObjective("Gate", Color.cyan);
            objective.Track = gatePosition;
            objective.Get<Text>("Title", t =>
            {
                t.text = "Reach the next room ---->";
                t.alignment = TextAnchor.MiddleCenter;
            });
        }

        private bool PlayerReady()
        {
            var players = GetTeam(0);
            int height = map.loading.Height / 2;
            Vector2 gatePosition = map.loading.CharacterPosition(new Vector2Int(3, height));
            for (int i = 0; i < players.Count; i++)
            {
                BaseController player = players[i];
                if (player.transform.position.x < gatePosition.x)
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

            const int bossEvery = 5;

            isBoss = (score + 3) % bossEvery == 0;
            SpawnsLeft = isBoss ? 1 :
                5 + (level * 2);

            if ((score + 4) % bossEvery == 0) BossRoom();
            else map.NextRoom(MapRoom.RandomSize());

            ClearDebris();
        }

        private void BossRoom()
        {
            map.NextRoom(80);
            map.loading.hasCenter = false;
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            map.QueuRoom(80);
            map.loading.hasCenter = false;

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
            if (!CheckEnemySpawns(out Vector2Int coords))
                return;

            if (isBoss) { 
                if (!SpawnBoss(1, coords, level)) return;
            }
            else SpawnRandom(1, coords, level);

            SpawnsLeft--;
            cooldown = 5;
        }

        protected override void ExtraMemberAdded(int team, BaseController controller)
        {
            UpdateProgress();
        }

        private bool SpaceForBossSpawn(Vector2Int coords, int size)
        {
            var room = map.current;

            bool __checkDir(Vector2Int dir)
            {
                for (int i = 1; i <= size; i++)
                    if (room.GetTile(coords + (dir * i)) != MapTileType.Empty)
                        return false;
                return true;
            }

            return  __checkDir(Vector2Int.up) &&
                    __checkDir(Vector2Int.down) &&
                    __checkDir(Vector2Int.left) &&
                    __checkDir(Vector2Int.right);
        }

        private EnemyController SpawnBoss(int team, Vector2Int pos, int level)
        {
            string bossName = bossesPaths.RandomElement();

            if(!Bosses.TryGetPrefab(bossName, out GameObject bossObject)) return null;

            EnemyController boss = bossObject.GetComponent<EnemyController>();

            if (!boss) return null;

            if (!SpaceForBossSpawn(pos, boss.settings.size)) return null;

            bossObject = Bosses.Instantiate(bossName);

            boss = bossObject.GetComponent<EnemyController>();
            boss.Set(level);
            boss.transform.position = map.current.CharacterPosition(pos);

            AddMember(team, boss);

            BossObjective(boss);

            return boss;
        }

        private void BossObjective(EnemyController boss)
        {
            objective = Objectives.CreateObjective("Boss Fight", new Color(255, 83, 31));
            objective.Get<Text>("Title", txt => txt.text = $"Eliminate {boss.Name}");
            objective.Get<Image>("Skull", img =>
            {
                img.sprite = Resources.Load<Sprite>($"Sprites/Bosses/{boss.Name}/Icon");
                if(!img.sprite) img.sprite = Resources.Load<Sprite>("Sprites/Bosses/Skull");

                img.transform.localScale = new Vector3(0.08f, 0.5f);
            });

            GameObject bar = Resources.Load<GameObject>("UI/ShieldHealthBar");
            if (!bar) return;

            bar = Object.Instantiate(bar);
            bar.name = $"{boss.Name} Health";
            bar.GetComponent<AspectRatioFitter>().enabled = false;
            var foreground = bar.transform.Find("Foreground");
            foreground.GetComponent<Image>().sprite =
                Resources.Load<Sprite>($"Sprites/Bosses/{boss.Name}/Foreground");
            foreground.localScale = new Vector3(0.97f, 0.8f, 1);

            objective.Add(bar);

            bar.transform.localScale = new Vector3(20, 0.9f, 1);
            boss.SetHealthBar(bar.transform);
        }

        private void BossDrop(EnemyController boss)
        {
            Objectives.Remove(objective);
            GameObject crate = Object.Instantiate(cratePrefab);
            crate.transform.position = boss.transform.position;
            crate.transform.localScale = new Vector3(0.25f, 0.25f, 1);
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

        public void ControllerLevelUp(BaseController controller)
        {
            if(controller is PlayerController)
                controller.stats.MaxHealthPoints(controller.Level);
        }
    }
}
