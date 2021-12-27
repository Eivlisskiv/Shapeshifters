using UnityEngine;
using System;
using Scripts.OOP.Database;
using Scripts.OOP.Game_Modes.CustomLevels;

namespace Scripts.OOP.Game_Modes.Story
{
    public class StoryLevel : CustomLevel
    {
        private readonly StorySettings story;

        public StoryLevel(StorySettings levelSettings, MainMenuHandler menu,
            MapHandler map, int extra_teams = 0) : base(levelSettings, menu, map,
                ChapterSettings.GetTeams(extra_teams)) 
        {
            story = levelSettings;
        }

        public override int LoadProgress()
        {
            StoryProgress progress = StoryProgress.Load(story.Chapter, story.Episode, false);
            return progress?.TopScore ?? 0;
        }

        public override void SaveProgress()
        {
            if (!LevelCompleted) return;

            StoryProgress progress = StoryProgress.Load(story.Chapter, story.Episode, true);
            if (Score > progress.TopScore)
                progress.TopScore = Score;

            if (progress.BestTimeSeconds == 0 || game_timer < progress.BestTimeSeconds)
                progress.BestTimeSeconds = game_timer;

            PlayerController player = (PlayerController)GetTeam(0)[0];

            progress.LevelReached = player.Level;
            progress.Experience = player.Xp;

            progress.Perks = player.perks.Serialize(progress.Id);

            progress.Weapon = player.Weapon.GetType().Name;

            progress.Save();
        }

        public override void UpdateMenu(MainMenuHandler menu) { }

        protected override PlayerController SpawnPlayer()
        {
            PlayerController player = base.SpawnPlayer();
            if (story.Episode > 1)
            {
                StoryProgress previous = StoryProgress.Load(story.Chapter, story.Episode - 1, true);
                player.LoadProgress(previous);
            }
            return player;
        }
    }

    static class ChapterSettings
    {
        static readonly Color[] Teams =
        {
            Color.green,    //Player team
            Color.red,      //Main Antagonist
            Color.yellow,   //Side Antagonist
            Color.blue,     //Infected
        };

        public static Color[] GetTeams(int extra)
        {
            int l = Math.Min(2 + extra, Teams.Length);
            if (l == Teams.Length) return Teams;

            Color[] teams = new Color[l];
            Array.Copy(Teams, teams, teams.Length);
            return teams;
        }
    }
}
