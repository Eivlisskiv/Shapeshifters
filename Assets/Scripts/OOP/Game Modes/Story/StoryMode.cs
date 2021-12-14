using IgnitedBox.EventSystem;
using Scripts.UI.InGame.Objectives;
using Scripts.UI.InGame.Objectives.ObjectivePresets;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Position;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Game_Modes.Story
{
    public class StoryMode : AGameMode
    {
        private float game_timer;

        private bool ongoing;
        private bool LevelCompleted => ObjectivesProgress >= levelSettings.main.Length;

        protected StoryLevel levelSettings;
        protected int ObjectivesProgress { get; private set; }
        private int mapProgress;

        protected ObjectiveElement CurrentObjective { get; private set; }

        private readonly List<PlayerController> playersReady
            = new List<PlayerController>();

        public EventsHandler<System.Type> ObjectiveEvents
            { get; private set; }
            = new EventsHandler<System.Type>();

        public StoryMode(StoryLevel levelSettings, MainMenuHandler menu, 
            MapHandler map, params Color[] teamColors)
            : base(menu, map, teamColors)
        {
            description = levelSettings.description;
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
            mapProgress = -1;
            NextMap();
            
            //How to identify which map the player is in to spawn enemies and objective props? (current?)
        }

        protected void NextMap()
        {
            if (mapProgress + 1 >= levelSettings.maps.Length) return;
            mapProgress++;
            map.QueuRoom(levelSettings.maps[mapProgress]);
        }

        public override void OnLoaded()
        {
            //base only after loading props
            base.OnLoaded();
            NextMap();
            SpawnPlayer();

            ongoing = true;
        }

        protected override void OnReady() 
        {
            NextObjective();

            ObjectiveHandler.Instance.Events.Subscribe
                <ObjectiveHandler, ObjectiveElement>(
                ObjectiveHandler.EventTypes.Removed,
                ObjectiveCompleted);
        }

        private void SpawnPlayer()
        {
            PlayerController player = PlayerController.Instantiate
                (map.characterPrefab, map.uiPrefab,
                Camera.main, map.mainCanvas.transform);

            player.transform.position = map.current.MapPosition(levelSettings.playerSpawn);
            AddMember(0, player);
        }

        public override void MapEntered(RoomHandler room, Collider2D subject)
        {
            //The next map is the one we want to enter
            if (room != map.loading) return;
            PlayerController player = subject.gameObject.GetComponent<PlayerController>();
            if (!player || playersReady.Contains(player)) return;

            int count = playersReady.Count + 1;

            ObjectiveEvents.Invoke(typeof(Reach_Map), this, count);

            if (count == GetTeam(0).Count)
            {
                map.loading.OpenGate(false);
                playersReady.Clear();
                NextMap();
                return;
            }

            playersReady.Add(player);
        }

        public override void MapExited(RoomHandler room, Collider2D subject)
        {
            //The next map is the one we want to enter
            if (room != map.loading) return;
            PlayerController player = subject.gameObject.GetComponent<PlayerController>();
            if (!player) return;

            playersReady.Remove(player);

            ObjectiveEvents.Invoke(typeof(Reach_Map), this, playersReady.Count);

        }

        protected override void GameOver()
        {
            base.GameOver();
            ongoing = false;
            //Show score, show if level was completed
        }

        private void NextObjective()
        {
            ObjectivesProgress++;

            if(LevelCompleted)
            {
                GameOver();
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
