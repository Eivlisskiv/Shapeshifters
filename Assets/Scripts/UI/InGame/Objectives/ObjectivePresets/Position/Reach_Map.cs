using Scripts.OOP.Game_Modes.Story;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Position
{
    public class Reach_Map : ObjectivePreset
    {
        private int playerCount;

        private Text progress;

        public Reach_Map(GameObject element, ObjectiveData data) : base(element, data) { }

        public override void Initialize(ObjectiveData data)
        {
            base.Initialize(data);

            Track = Game.NextGate(true);
            Get<Text>("Title", (t) => t.text = LoadParam(data, 0, "Reach the next room.") );

            playerCount = Game.GetTeam(0).Count;

            progress = Get<Text>("Progress", (t) => t.text = $"0/{playerCount} players" );

            Game.ObjectiveEvents.Subscribe<StoryMode, int> 
                    (typeof(Reach_Map), MapProgress);
        }

        private void MapProgress(StoryMode game, int progress)
        {
            if (progress == playerCount)
            {
                Handler.Remove(this);
                return;
            }

            this.progress.text = $"{progress}/{playerCount} players";
        }
    }
}
