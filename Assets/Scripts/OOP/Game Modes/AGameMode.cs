using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using Scripts.OOP.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Game_Modes
{
    public abstract class AGameMode
    {
        private string _name;

#pragma warning disable IDE0074 // Use compound assignment
        public string Name => _name ??
            (_name = GetType().Name.Replace('_', ' '));
#pragma warning restore IDE0074 // Use compound assignment

        public string description = "...";

        protected string RessourcePath => $"GameMode/{Name}/";

        protected readonly string scoreNamePath;

        protected bool Loaded { get => loaded; }
        private bool loaded;
        
        public int Score { get => score; }
        protected int score;

        protected MapHandler map;
        private readonly MainMenuHandler menu;

        public int TeamCount => teams.Length;

        private readonly List<BaseController>[] teams;
        private readonly Color[] teamColors;
        private readonly Transform gameTransform;
        public readonly (Transform parent, Transform debris)[] teamTransforms;

        protected readonly private ResourceCache<GameObject> enemies
            = new ResourceCache<GameObject>("Enemies/");

        protected readonly Dictionary<string, (float, string[])> enemyTable;

        protected ObjectiveHandler Objectives { get; private set; }

        public int SavedScore()
            => PlayerPrefs.GetInt(scoreNamePath);

        public AGameMode(MainMenuHandler menu, MapHandler map,
            Dictionary<string, (float, string[])> enemyTable, params Color[] teamColors)
        {
            this.menu = menu;
            this.map = map;

            this.enemyTable = enemyTable;

            this.teamColors = teamColors;
            teams = new List<BaseController>[teamColors.Length];
            teamTransforms = new (Transform, Transform)[teamColors.Length];
            gameTransform = new GameObject("GameContent").transform;

            for (int i = 0; i < teams.Length; i++)
                InitiateTeam(i);

            GameModes.SetInstance(this);

            scoreNamePath = $"Score_{Name}";
        }

        private void InitiateTeam(int i)
        {
            teams[i] = new List<BaseController>();

            Transform parent = new GameObject($"Team {i}").transform;
            parent.SetParent(gameTransform);
            SetDebrisTransform(i, parent);
        }

        private void SetDebrisTransform(int i, Transform parent)
        {
            Transform debris = new GameObject("Debris").transform;
            debris.SetParent(parent.transform);
            teamTransforms[i] = (parent, debris);
        }

        protected void ClearDebris()
        {
            for (int i = 0; i < teamTransforms.Length; i++)
            {
                (Transform parent, Transform debris) = teamTransforms[i];
                Object.Destroy(debris.gameObject);
                SetDebrisTransform(i, parent);
            }
        }

        public abstract void OnUpdate();

        public void Clear()
        {
            Object.Destroy(gameTransform.gameObject);
        }

        protected EnemyController SpawnRandom(int team, Vector2Int coords, int level)
        {
            GameObject mob = GetEnemySpawn();
            mob.transform.position = map.current.CharacterPosition(coords);
            var enemy = mob.GetComponent<EnemyController>();
            enemy.Set(level);
            AddMember(team, enemy);
            return enemy;
        }

        private GameObject GetEnemySpawn()
        {
            if (enemyTable != null && enemyTable.Count > 0)
            {
                float r = Random.Range(0, 101);
                foreach (var v in enemyTable)
                {
                    if (r <= v.Value.Item1)
                        return enemies.Instantiate($"{v.Key}/{Randomf.Element(v.Value.Item2)}");
                }
            }

            return enemies.Instantiate("Regular/Tier1/Regular");
        }

        public void AddMember(int team, BaseController controller)
        {
            if(teams.Length <= team)
            {
                Debug.LogWarning($"There is no team #{team}");
                return;
            }

            controller.transform.SetParent(teamTransforms[team].parent);
            teams[team].Add(controller);

            controller.SetColor(0, teamColors[team]);
            controller.team = team;
        }

        public List<BaseController> GetTeam(int team) 
            => teams[team];

        public List<BaseController> GetEnemies(int ofTeam)
        {
            if (teams.Length == 2) return GetTeam(ofTeam == 0 ? 1 : 0);

            List<BaseController> enemies = new List<BaseController>();
            for (int i = 0; i < teams.Length; i++)
            {
                if (i == ofTeam) continue;
                List<BaseController> t = teams[i];
                enemies.AddRange(t);
            }

            return enemies;
        }

        protected void PauseControllers(bool value)
        {
            int teamCount = TeamCount;
            for (int i = 0; i < teamCount; i++)
            {
                var team = GetTeam(i);
                for (int i1 = 0; i1 < team.Count; i1++)
                {
                    BaseController m = team[i1];
                    m.DisableController(value);
                }
            }
        }

        public virtual void MemberDestroyed(BaseController member)
        {
            var team = teams[member.team];
            team.Remove(member);
        }

        public virtual void StartMap()
        {
            map.enabled = true;
            map.StartMap();
        }

        public virtual void OnLoaded()
        {
            loaded = true;
            menu.SetStartButton(true);
            menu.container.SetActive(false);

            Objectives = menu.SpawnGameUI(description, OnReady);

            PauseHandler.SetControl(true);
        }

        protected abstract void OnReady();

        public virtual void PlayerElimenated(PlayerController player)
            => GameOver();

        protected void GameOver()
        {
            Objectives.transform.Tween<Transform, Vector3, PositionTween>
                (Objectives.transform.localPosition + new Vector3(0, 200, 0),
                0.5f, callback: () => Object.Destroy(Objectives));
            MainMenuHandler.GameOver();
        }

        public virtual void EndGame()
        {
            PauseHandler.SetControl(false);
            map.Clear();
        }
    }
}
