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

        private EnemyController boss;
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
            : base(menu, map, new ExpTable<PathTable>(1.15,
                new PathTable("Regular/Tier1/", "Regular")
                ), Color.green, Color.red)
        {
            cooldown = 5;
            level = 1;
            SpawnsLeft = 5;

            GameObject shopPrefab = Resources.Load<GameObject>(RessourcePath + "Shop");
            cratePrefab = Resources.Load<GameObject>(RessourcePath + "Crate");
            shop = Object.Instantiate(shopPrefab, menu.transform.parent.GetChild(0)).GetComponent<ShopHandler>();
            shop.gameObject.SetActive(false);

            bossesPaths = new string[]
            { "Pyramid", "Number Four", "PRDS" };
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

            if (isBoss && member == boss) BossDrop();

            if (GetTeam(1).Count == 0 && SpawnsLeft == 0) FinishWave();
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
            objective.Get<Text>("Title", t => t.text = "Eliminate all the enemies.", 1.3f);
            objective.Get<Text>("Objective", t =>
            {
                t.text = "Enemies Left: ";
                t.alignment = TextAnchor.MiddleCenter;
            });

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
                Camera.main, map.inGameUI.transform);

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

        private void SpawnBoss(int team, int level)
        {
            string bossName = bossesPaths.DropOne(out _);

            Color color = new Color(255, 83, 31);
            CustomLevels.ObjectiveData data = new CustomLevels.ObjectiveData
                ("Target Enemy", color, 10, "Boss Fight", $"Boss/{bossName}", team, level);

            Target_Enemy obj = ObjectiveHandler.Instance.CreateObjective<Target_Enemy>(data.id, data.color, data);
            objective = obj;
            boss = obj.Target;
        }

        private void BossDrop()
        {
            maxSpawns++;
            points += level;
            Objectives.Remove(objective);
            GameObject crate = Object.Instantiate(cratePrefab);
            crate.transform.position = boss.transform.position;
            crate.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            AddSpawns("Special/", boss.name);
            boss = null;
        }

        public void ControllerLevelUp(BaseController controller)
        {
            if (controller is PlayerController)
            {
                controller.stats.MaxHealthPoints(controller.Level);
            }
        }

        protected override void ScoreChanged()
        {
            //3, 8
            switch (Score)
            {
                case 1: 
                    //All tiers must be added before Special is added
                    //as Special must be last to have the smallest drop chance
                    AddSpawns("Regular/Tier1/", "Sniper");
                    AddSpawns("Regular/Tier2/", "Gunner");
                    break;
                //after 2 is a boss fight
                //after 3 unlocks boss fight at 2
                case 4: AddSpawns("Regular/Tier1/", "Bomber"); break;
                case 5: AddSpawns("Regular/Tier1/", "Tank"); break;
                case 6: AddSpawns("Regular/Tier2/", "Flamer"); break;
                //after 7 is a boss fight
                //after 8 unclocks boss
                case 9: AddSpawns("Regular/Tier2/", "Pirate"); break;
            }
        }

        protected override void AddSpawns(string category, params string[] names)
        {
            int sub = spawnTable.FindIndex(t => t.Name == category);
            if (sub < 0) spawnTable.Add(new PathTable(category, names));
            else
            {
                PathTable tab = spawnTable[sub];
                for (int i = 0; i < names.Length; i++)
                {
                    string name = names[i];
                    if (tab.Contains(name)) continue;

                    spawnTable[sub].Add(names[i]);
                }
            }
        }
    }
}
