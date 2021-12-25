using IgnitedBox.EventSystem;
using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.UnityUtilities;
using Scripts.OOP.Utils;
using Scripts.UI.InGame.Objectives;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Game_Modes
{
    public abstract class AGameMode
    {

        public readonly Color[] teamColors;

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
        
        public int Score 
        {
            get => score; 
            set
            {
                score = value;
                ScoreChanged();
            }
        }
        private int score;

        protected MapHandler map;
        private readonly MainMenuHandler menu;

        public int TeamCount => teams.Length;

        private readonly List<BaseController>[] teams;
        private readonly Transform gameTransform;
        public readonly (Transform parent, Transform debris)[] teamTransforms;

        protected readonly private ResourceCache<GameObject> enemies
            = new ResourceCache<GameObject>("Enemies/");

        protected ObjectiveHandler Objectives { get; private set; }

        public AGameMode(MainMenuHandler menu, MapHandler map, params Color[] teamColors)
        {
            this.menu = menu;
            this.map = map;

            this.teamColors = teamColors;
            teams = new List<BaseController>[teamColors.Length];
            teamTransforms = new (Transform, Transform)[teamColors.Length];
            gameTransform = new GameObject("GameContent").transform;

            for (int i = 0; i < teams.Length; i++)
                InitiateTeam(i);

            GameModes.SetInstance(this);

            scoreNamePath = $"Score_{Name}";
        }

        public abstract int LoadProgress();

        public abstract void SaveProgress();

        public abstract void UpdateMenu(MainMenuHandler menu);

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

        public EnemyController SpawnEnemy(GameObject mob, int team, int level, Vector2Int? coords = null)
        {
            if (mob.IsPrefab()) mob = Object.Instantiate(mob);
            mob.transform.position = map.current.MapPosition(coords ?? map.current.RandomSpawn());
            var enemy = mob.GetComponent<EnemyController>();
            enemy.Set(level);
            AddMember(team, enemy);
            return enemy;
        }

        public void AddMember(int team, BaseController controller, bool isExtra = false)
        {
            if(teams.Length <= team)
            {
                Debug.LogWarning($"There is no team #{team}");
                return;
            }

            controller.transform.SetParent(teamTransforms[team].parent);
            teams[team].Add(controller);

            controller.SetColor(0, teamColors[team]);
            controller.Team = team;

            if (isExtra) ExtraMemberAdded(team, controller);
        }

        protected virtual void ExtraMemberAdded(int team, BaseController controller) { }

        protected virtual void ScoreChanged() { }

        public ObjectiveTracking? NextGate(bool openGame)
        {
            Vector2 pos = map.current.MapPosition(map.current.OpenGate(openGame));
            Vector2 mid = map.loading.MapPosition(new Vector2Int(map.loading.Width / 2, map.loading.Height / 2));
            return pos + ((mid - pos).normalized * 9);
        }

        public virtual void MapEntered(RoomHandler room, Collider2D subject) { }
        public virtual void MapExited(RoomHandler room, Collider2D subject) { }

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

        public void PauseControllers(bool value)
        {
            int teamCount = TeamCount;
            for (int i = 0; i < teamCount; i++)
            {
                var team = GetTeam(i);
                for (int i1 = 0; i1 < team.Count; i1++)
                {
                    team[i1].DisableController(value);
                }
            }
        }

        public virtual void MemberDestroyed(BaseController member)
        {
            var team = teams[member.Team];
            team.Remove(member);
        }

        protected abstract void OnMapStarted();

        public void StartMap()
        {
            OnMapStarted();
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

        protected virtual void GameOver()
        {
            Objectives.transform.Tween<Transform, Vector3, PositionTween>
                (Objectives.transform.localPosition + new Vector3(0, 200, 0),
                0.5f, callback: () => Object.Destroy(Objectives.gameObject));
            menu.GameOver();
        }

        public virtual void EndGame()
        {
            PauseHandler.SetControl(false);
            map.Clear();
        }
    }
}
