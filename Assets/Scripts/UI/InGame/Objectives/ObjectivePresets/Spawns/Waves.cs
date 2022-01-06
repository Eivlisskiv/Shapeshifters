using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns
{
    public class Waves : ObjectivePreset, IControllerElimenated
    {
        int wave;

        int team;

        private (string, int)[][] waves;

        private Text progress;
        private Text wprogress;

        List<BaseController> spawns;

        public Waves(GameObject element, ObjectiveData data) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            spawns = new List<BaseController>();

            team = LoadParam(data, 1, 1);

            waves = LoadParam(data, 2, new (string, int)[][] { new (string, int)[] { ("Regular/Tier 1/Regular", 0) } });

            Text title = base.Initialize(data);

            progress = Get<Text>("Progress", t =>
            {
                t.text = GetControllersProgress();
                t.alignment = TextAnchor.MiddleCenter;
            });

            if (team != 0)
            {
                wprogress = Get<Text>("Waves", t =>
                {
                    t.text = GetWaveProgress();
                    t.alignment = TextAnchor.MiddleCenter;
                });

                ChangeWidth("Waves", 0.8f);
            }

            Game.ObjectiveEvents.Subscribe<CustomLevel, BaseController>
                (typeof(IControllerElimenated), Progress);

            ChangeWidth("Title", 1.3f);

            return title;
        }

        protected override void OnReady()
        {
            base.OnReady();
            SpawnWave();
        }

        public void Progress(AGameMode mode, BaseController elimenated)
        {
            if (!spawns.Contains(elimenated)) return;

            spawns.Remove(elimenated);
            progress.text = GetControllersProgress();

            Bounce();

            //Enemies left
            if (spawns.Count > 0) return;

            wave++;
            if (wprogress) wprogress.text = GetWaveProgress();
            if (wave < waves.Length)
            {
                Game.Score++;
                SpawnWave();
                return;
            }

            //All waves finished
            Handler.Remove(this);
        }

        private void SpawnWave()
        {
            for (int i = 0; i < waves[wave].Length; i++)
            {
                (string path, int level) = waves[wave][i];
                GameObject obj = Resources.Load<GameObject>("Enemies/" + path);
                if(!obj) obj = Resources.Load<GameObject>("Enemies/Regular/Tier1/Regular");
                spawns.Add(Game.SpawnEnemy(obj, team, level));
            }

            progress.text = GetControllersProgress();
        }

        private string GetControllersProgress()
            => $"{spawns.Count} {(team == 0 ? "Allies" : "Hostiles")} left.";

        private string GetWaveProgress()
            => $"[Wave {wave + 1}/{waves.Length}]";
    }
}
