
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.GameModes
{
    public abstract class AGameMode
    {
        
        public static AGameMode GameMode
        { get => _instance; }
        private static AGameMode _instance;

        public static void Run<T>(Action<T> func)
        {
            if (_instance is T mode) func(mode);
        }

        public static Transform GetDebrisTransform(int team)
            => _instance?.teamTransforms[team].debris;

        protected bool Loaded { get => loaded; }
        private bool loaded;
        
        protected int score;

        protected MapHandler map;
        private readonly MainMenuHandler menu;

        public int TeamCount => teams.Length;

        private readonly List<BaseController>[] teams;
        private readonly Color[] teamColors;
        private readonly Transform gameTransform;
        private readonly (Transform parent, Transform debris)[] teamTransforms;

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

            _instance = this;
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
                UnityEngine.Object.Destroy(debris.gameObject);
                SetDebrisTransform(i, parent);
            }
        }

        public abstract void OnUpdate();

        public void Clear()
        {
            UnityEngine.Object.Destroy(gameTransform.gameObject);
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

            controller.Color = teamColors[team];
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
        }

        public virtual void GameOver()
        {

        }

        public virtual void EndGame()
        {
            map.Clear();
        }
    }
}
