using Scripts.OOP.Game_Modes.Story;
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

        List<BaseController> spawns;

        public Waves(GameObject element, ObjectiveData data) : base(element, data) { }

        protected override void Initialize(ObjectiveData data)
        {
            spawns = new List<BaseController>();

            base.Initialize(data);

            Get<Text>("Title", (t) => t.text = LoadParam(data, 0, "AI Waves"));

            team = LoadParam(data, 1, 1);

            waves = LoadParam(data, 2, new (string, int)[][] { new (string, int)[] { ("Regular/Tier 1/Regular", 0) } });

            progress = Get<Text>("Progress", t => t.text = GetProgress());

            Game.ObjectiveEvents.Subscribe<CustomLevel, BaseController>
                (typeof(IControllerElimenated), Progress);
        }

        protected override void OnReady()
        {
            base.OnReady();
            SpawnWave();
        }

        public void Progress(CustomLevel mode, BaseController elimenated)
        {
            if (!spawns.Contains(elimenated)) return;

            spawns.Remove(elimenated);
            progress.text = GetProgress();

            //Enemies left
            if (spawns.Count != 0) return;

            wave++;
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

            progress.text = GetProgress();
        }

        private string GetProgress()
            => $"{spawns.Count} {(team == 0 ? "Allies" : "Hostiles")} left.";
    }
}
