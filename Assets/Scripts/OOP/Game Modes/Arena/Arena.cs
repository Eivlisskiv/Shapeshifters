using Scripts.OOP.Perks;
using Scripts.OOP.TileMaps.Procedural;
using Scripts.OOP.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using IgnitedBox.Random.DropTables;
using IgnitedBox.Random.DropTables.CategorizedTable;
using Scripts.UI.InGame.Objectives;

namespace Scripts.OOP.Game_Modes.Arena
{
    public class Arena : ArcadeMode<ExpTable<PathTable>>, IControllerLevelUp, IElimination
    {
        private bool spawns = false;
        private float spawnCooldown;
        private int level;

        private ObjectiveElement mainObjective;
        private int eliminations;
        private Text progress;

        private int switchWeapon;

        public Arena(MainMenuHandler menu, MapHandler map) 
            : base(menu, map, new ExpTable<PathTable>(1.01, 
                new PathTable("Regular/Tier1/", "Regular")
                ), Color.green, Color.red)
        {
            spawnCooldown = 5;
            level = 1;
        }

        public override void OnUpdate()
        {
            if (map.Current == null || !map.Current.Loaded) return;

            if (!spawns) return;

            if(spawnCooldown > 0)
            {
                spawnCooldown -= Time.deltaTime;
                return;
            }

            if (GetTeam(1).Count < 3)
                SpawnEnemy(GetRandomEnemy(), 1, level);
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

        protected override void OnReady()
        {
            mainObjective = Objectives.CreateObjective("Main", Color.red,
                func: objElement =>
                {
                    objElement.Get<Text>("Title", t => t.text = "Eliminate enemies.", 1.5f);
                    objElement.Get<Text>("Objective", t =>
                    {
                        t.text = "Eliminations: ";
                        t.alignment = TextAnchor.MiddleCenter;
                    });
                });

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
                Camera.main, map.inGameUI.transform);

            player.transform.position = map.Current.MapPosition(new Vector2Int
                (ProceduralMapRoom.spacing + (ProceduralMapRoom.borderWidth * 2) - 1, map.Current.Width / 4));

            AddMember(0, player);
        }

        public void ControllerLevelUp(BaseController controller)
        {
            if (controller is PlayerController player)
            {
                controller.stats.MaxHealthPoints(controller.Level);

                level = player.Level;
                Score = player.Level;

                if (player.Level == 1 || player.Level % 5 == 0)
                {
                    Perk perk = PerksHandler.Random();
                    player.AddPerk(perk);
                }
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
