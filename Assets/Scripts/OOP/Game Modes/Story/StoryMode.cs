using Scripts.UI.InGame.Objectives;
using Scripts.UI.InGame.Objectives.ObjectivePresets;
using UnityEngine;

namespace Scripts.OOP.Game_Modes.Story
{
    public abstract class StoryMode : AGameMode
    {
        private float game_timer;

        private bool ongoing;
        private bool LevelCompleted => ObjectivesProgress >= levelSettings.main.Length;

        protected StoryLevel levelSettings;
        protected int ObjectivesProgress { get; private set; }
        protected ObjectiveElement CurrentObjective { get; private set; }

        public StoryMode(StoryLevel levelSettings, MainMenuHandler menu, 
            MapHandler map, params Color[] teamColors)
            : base(menu, map, teamColors)
        {
            this.levelSettings = levelSettings;
            ObjectivesProgress = -1;
        }

        public override void OnUpdate()
        {
            if(ongoing) game_timer += Time.deltaTime;
        }

        protected override void OnMapStarted()
        {
            //Load the first map
            map.QueuRoom(levelSettings.maps[0]);
            //Map handling needs to be figure out
            //When should the next map be loaded?
            //How to identify which map the player is in to spawn enemies and objective props
        }

        public override void OnLoaded()
        {
            //base only after loading props
            base.OnLoaded();

            ongoing = true;
        }

        protected override void OnReady() 
        {
            //3. Load the first objective and spawn the player

            NextObjective();

            ObjectiveHandler.Instance.Events.Subscribe
                <ObjectiveHandler, ObjectiveElement>(
                ObjectiveHandler.EventTypes.Removed,
                ObjectiveCompleted);
        }

        public override void EndGame()
        {
            base.EndGame();
            ongoing = false;
            //Show score, show if level was completed
        }

        private void NextObjective()
        {
            ObjectivesProgress++;

            if(LevelCompleted)
            {
                EndGame();
                return;
            }

            ObjectiveData data = levelSettings.main[ObjectivesProgress];
            CurrentObjective = ObjectivePreset.Create(data);
        }

        private void ObjectiveCompleted(ObjectiveHandler _, ObjectiveElement obj)
        {
            if(obj == CurrentObjective)
            {
                NextObjective();
            }
        }
    }
}
