using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Position
{
    public class Reach_Map : ObjectivePreset
    {
        private int playerCount;

        private Text progress;

        public Reach_Map(GameObject element, ObjectiveData data) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            Track = Game.NextGate(true);

            playerCount = Game.GetTeam(0).Count;

            Text title = base.Initialize(data);

            if (playerCount > 1)
            {
                progress = Get<Text>("Progress", (t) =>
                {
                    t.alignment = TextAnchor.MiddleCenter;
                    t.text = $"0/{playerCount} players";
                });
            }

            Game.ObjectiveEvents.Subscribe<CustomLevel, int> 
                    (typeof(Reach_Map), MapProgress);

            return title;
        }

        private void MapProgress(CustomLevel game, int progress)
        {
            if (progress == playerCount)
            {
                Completed();
                return;
            }

            if (this.progress)
                this.progress.text = $"{progress}/{playerCount} players";
        }
    }
}
