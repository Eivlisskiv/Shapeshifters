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

        protected override void Initialize(ObjectiveData data)
        {
            base.Initialize(data);

            Track = Game.NextGate(true);
            Get<Text>("Title", (t) =>
            {
                t.alignment = TextAnchor.MiddleCenter;
                t.text = LoadParam(data, 0, "Reach the next room.");
            });

            playerCount = Game.GetTeam(0).Count;

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
        }

        private void MapProgress(CustomLevel game, int progress)
        {
            if (progress == playerCount)
            {
                Handler.Remove(this);
                return;
            }

            if (this.progress)
                this.progress.text = $"{progress}/{playerCount} players";
        }
    }
}
