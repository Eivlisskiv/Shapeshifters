using IgnitedBox.Random.DropTables;
using IgnitedBox.Random.DropTables.CategorizedTable;
using Scripts.OOP.TileMaps;
using Scripts.OOP.TileMaps.Procedural;
using Scripts.OOP.Utils;
using Scripts.UI.InGame.Objectives;
using Scripts.UI.InGame.Objectives.ObjectivePresets;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.Game_Modes.Rogue
{
    public class Rogue : ArcadeMode<ExpTable<PathTable>>, IRogueMenu, IControllerLevelUp
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

        private int maxSpawns = 3;

        private void UpdateProgress()
        {
            if (progress) progress.text = (GetTeam(1).Count + spawnsLeft).ToString();
        }

        private int spawnsLeft;

        private Stage stage;

        private readonly GameObject cratePrefab;
        private readonly ShopHandler shop;
        private readonly DropTable<string> bossesPaths;

        private readonly List<PlayerController> playersReady
            = new List<PlayerController>();

        public Rogue(MainMenuHandler menu, MapHandler map)
            : base(menu, map, new ExpTable<PathTable>(1.01,
                new PathTable("Regular/Tier1/", "Regular")
                ), Color.green, Color.red)
        {
            cooldown = 5;
            level = 1;
            SpawnsLeft = 5;

            GameObject shopPrefab = Resources.Load<GameObject>(RessourcePath + "Shop");
            cratePrefab = Resources.Load<GameObject>(RessourcePath + "Crate");
            shop = Object.Instantiate(shopPrefab, menu.transform.parent).GetComponent<ShopHandler>();
            shop.gameObject.SetActive(false);

            bossesPaths = new string[]
            { "Pyramid", "Number Four" };
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
            Score++;
            if (!map.Loading.Loaded) map.Loading.SetTilesPerFrame(50);
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

            objective = Objectives.CreateObjective("Gate", Color.cyan);
            objective.Track = NextGate(true);
            objective.Get<Text>("Title", t =>
            {
                t.text = "Reach the next room";
                t.alignment = TextAnchor.MiddleCenter;
            });
        }

        public override void MapEntered(RoomHandler room, Collider2D subject)
        {
            //The next map is the one we want to enter
            if (stage != Stage.PassGate || room != map.Loading) return;
            PlayerController player = subject.gameObject.GetComponent<PlayerController>();
            if (!player || playersReady.Contains(player)) return;

            if(playersReady.Count + 1 == GetTeam(0).Count)
            {
                playersReady.Clear();
                NextWave();
                return;
            }

            playersReady.Add(player);
        }

        public override void MapExited(RoomHandler room, Collider2D subject)
        {
            //The next map is the one we want to enter
            if (stage != Stage.PassGate || room != map.Loading) return;
            PlayerController player = subject.gameObject.GetComponent<PlayerController>();
            if (!player) return;

            playersReady.Remove(player);
        }

        private void NextWave()
        {
            stage = Stage.Waves;

            Objectives.Remove(objective);
            objective = Objectives.Current;

            cooldown = 10;
            level++;

            const int bossEvery = 5;

            isBoss = (Score + 3) % bossEvery == 0;
            SpawnsLeft = isBoss ? 1 :
                5 + maxSpawns;

            if ((Score + 4) % bossEvery == 0) BossRoom();
            else map.NextProceduralRoom(ProceduralMapRoom.RandomSize());

            ClearDebris();
        }

        private void BossRoom()
        {
            map.NextProceduralRoom(80);
            map.Loading.hasCenter = false;
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            map.QueuRoom<CaveRoom>(80);
            map.Loading.hasCenter = false;

            SpawnPlayer();
        }

        protected override void OnMapStarted()
        {
            map.QueuRoom<CaveRoom>(80);
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

            player.transform.position = map.Current.MapPosition(new Vector2Int
                (ProceduralMapRoom.spacing + (ProceduralMapRoom.borderWidth * 2) - 1, map.Current.Width / 4));
            shop.player = player;
            AddMember(0, player);
        }

        private void SpawnEnemy()
        {
            if (GetTeam(1).Count >= maxSpawns)
                return;

            if (isBoss) SpawnBoss(1, level);
            else SpawnEnemy(GetRandomEnemy(), 1, level);

            SpawnsLeft--;
            cooldown = 5;
        }

        protected override void ExtraMemberAdded(int team, BaseController controller)
        {
            UpdateProgress();
        }

        private bool SpaceForBossSpawn(Vector2Int coords, int size)
        {
            var room = map.Current;

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

        private void SpawnBoss(int team, int level)
        {
            string bossName = bossesPaths.DropOne(out _);

            Color color = new Color(255, 83, 31);
            CustomLevels.ObjectiveData data = new CustomLevels.ObjectiveData
                ("Target Enemy", color, 10, "Boss Fight", $"Boss/{bossName}", team, level);

            objective = ObjectivePreset.Create(data);
        }

        private void BossDrop(EnemyController boss)
        {
            maxSpawns++;
            points += level;
            Objectives.Remove(objective);
            GameObject crate = Object.Instantiate(cratePrefab);
            crate.transform.position = boss.transform.position;
            crate.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        }

        public void ControllerLevelUp(BaseController controller)
        {
            if (controller is PlayerController)
            {
                controller.stats.MaxHealthPoints(controller.Level);
            }
        }

        protected override void AddSpawns(string category, params string[] names)
        {
            int sub = spawnTable.FindIndex(t => t.Name == category);
            if (sub < 0) spawnTable.Add(new PathTable(category, names));
            else spawnTable[sub].AddRange(names);
        }
    }
}
