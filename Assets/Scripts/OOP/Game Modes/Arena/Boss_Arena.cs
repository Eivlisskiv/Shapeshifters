using IgnitedBox.Random.DropTables.CategorizedTable;
using Scripts.OOP.TileMaps.Procedural;
using Scripts.UI.InGame.Objectives;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.Game_Modes.Arena
{
    public class Boss_Arena : ArcadeMode<PathTable>, IElimination
    {
        private bool spawns = false;

        private Text countProgress;
        private int eliminations;

        private Target_Enemy bossObjective;

        private ObjectiveElement minionsObjective;
        private Text minionProgress;

        private float spawnCooldown = 3;

        private List<EnemyController> extras;

        public Boss_Arena(MainMenuHandler menu, MapHandler map)
            : base(menu, map, new PathTable("Boss/", "PRDS"),
                  Color.green, Color.red) 
        {
            extras = new List<EnemyController>();
        }

        public override void OnUpdate()
        {
            if (map.Current == null || !map.Current.Loaded) return;

            if (!spawns) return;

            if (spawnCooldown <= 0) return;

            spawnCooldown -= Time.deltaTime;

            if (spawnCooldown > 0) return;

            SpawnBoss(1, eliminations / 3);
        }

        public void Elimination(BaseController victim, BaseController killer)
        {
            if (!victim || !killer) return;

            if (victim.perks.Count > 0)
                killer.AddPerk(victim.perks.RandomDrop());
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

        private void SpawnPlayer()
        {
            PlayerController player = PlayerController.Instantiate
                (map.characterPrefab, map.uiPrefab,
                Camera.main, map.inGameUI.transform);

            player.transform.position = map.Current.MapPosition(new Vector2Int
                (ProceduralMapRoom.spacing + (ProceduralMapRoom.borderWidth * 2) - 1, map.Current.Width / 4));

            AddMember(0, player);
        }

        protected override void OnReady()
        {
            Objectives.CreateObjective("Main", Color.red,
                func: objElement =>
                {
                    objElement.Get<Text>("Title", t => t.text = "Survive the bosses!", 1.5f);
                    countProgress = objElement.Get<Text>("Count", t =>
                    {
                        t.text = "0";
                        t.alignment = TextAnchor.MiddleCenter;
                    });
                });

            spawns = true;
        }

        public override void MemberDestroyed(BaseController member)
        {
            base.MemberDestroyed(member);

            if (member is PlayerController player)
            {
                player.cam.Detach();
                return;
            }

            bool wasBoss = member == bossObjective?.Target;
            int count = GetTeam(1).Count;

            if (wasBoss) //Boss was eliminated
            {
                Objectives.Remove(bossObjective);

                eliminations++;
                countProgress.text = eliminations.ToString();

                if(count > 0) //There are minions left
                {
                    minionsObjective = Objectives.CreateObjective("Clean Up", Color.red, func: objElement =>
                    {
                        objElement.Get<Text>("Title", t => t.text = "Eliminate the Boss' minions", 2);
                        minionProgress = objElement.Get<Text>("Progress", t => t.text = count.ToString(), 1);
                    });

                    return;
                }

                spawnCooldown = 5;
                return;
            }

            if(count > 0) //Not boss and more minions left
            {
                if (minionProgress) minionProgress.text = count.ToString();
                return;
            }

            //Extra enemies were wiped
            Objectives.Remove(minionsObjective);

            spawnCooldown = 10;
        }

        protected override void ExtraMemberAdded(int team, BaseController controller)
        {
            base.ExtraMemberAdded(team, controller);
            if (team == 1 && minionProgress) minionProgress.text = GetTeam(1).Count.ToString();
        }

        private void SpawnBoss(int team, int level)
        {
            string bossPath = spawnTable.DropOne(out _);

            Color color = new Color(255, 83, 31);
            CustomLevels.ObjectiveData data = new CustomLevels.ObjectiveData
                ("Target Enemy", color, 10, "Boss Fight", bossPath, team, level);

            bossObjective = ObjectiveHandler.Instance
                .CreateObjective<Target_Enemy>(data.id, data.color, data);

            bossObjective.Track = bossObjective.Target.transform;
        }

        protected override void AddSpawns(string category, params string[] names) { }
    }
}
