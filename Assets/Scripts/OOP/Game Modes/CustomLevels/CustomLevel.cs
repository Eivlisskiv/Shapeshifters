﻿using Scripts.UI.InGame.Objectives;
using Scripts.UI.InGame.Objectives.ObjectivePresets;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Game_Modes.Story
{
    public abstract partial class CustomLevel : AGameMode
    {
        protected float game_timer { get; private set; }

        private bool ongoing;
        protected bool LevelCompleted => ObjectivesProgress >= levelSettings.Main.Length;

        protected LevelSettings levelSettings;
        protected int ObjectivesProgress { get; private set; }
        private int mapProgress;

        protected ObjectiveElement CurrentObjective { get; private set; }

        private readonly List<PlayerController> playersReady
            = new List<PlayerController>();

        public CustomLevel(LevelSettings levelSettings, MainMenuHandler menu, 
            MapHandler map, params Color[] teamColors)
            : base(menu, map, teamColors)
        {
            description = levelSettings.Description;
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
        }

        protected void NextMap()
        {
            if (mapProgress + 1 >= levelSettings.Maps.Length) return;
            mapProgress++;
            map.NextRoom(levelSettings.Maps[mapProgress], false);
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

        protected virtual PlayerController SpawnPlayer()
        {
            PlayerController player = PlayerController.Instantiate
                (map.characterPrefab, map.uiPrefab,
                Camera.main, map.mainCanvas.transform);

            player.transform.position = map.current.MapPosition(levelSettings.PlayerSpawn);
            AddMember(0, player);
            return player;
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
            Score++;
            if(LevelCompleted)
            {
                GameOver();
                return;
            }

            ObjectiveData data = levelSettings.Main[ObjectivesProgress];
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