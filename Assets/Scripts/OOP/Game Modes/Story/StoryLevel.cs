using UnityEngine;
using System;
using Scripts.OOP.Database;

namespace Scripts.OOP.Game_Modes.Story
{
    public abstract class StoryLevel : CustomLevel
    {
        public StoryLevel(LevelSettings levelSettings, MainMenuHandler menu,
            MapHandler map, int extra_teams = 0) : base(levelSettings, menu, map,
                ChapterSettings.GetTeams(extra_teams)) { }

        public override int LoadProgress()
        {
            StoryProgress progress = StoryProgress.Load(levelSettings.Chapter, levelSettings.Episode);
            return progress.TopScore;
        }

        public override void SaveProgress()
        {
            StoryProgress progress = StoryProgress.Load(levelSettings.Chapter, levelSettings.Episode);
            progress.LastScore = Score;
            if (Score > progress.TopScore)
                progress.TopScore = Score;
            progress.Save();
        }

        public override void UpdateMenu(MainMenuHandler menu)
        {
            
        }

        protected override PlayerController SpawnPlayer()
        {
            PlayerController player = base.SpawnPlayer();
            if (levelSettings.Episode > 1)
            {
                StoryProgress previous = StoryProgress.Load(levelSettings.Chapter, levelSettings.Episode - 1);
                //Load up all of the progress from the previous story;
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
