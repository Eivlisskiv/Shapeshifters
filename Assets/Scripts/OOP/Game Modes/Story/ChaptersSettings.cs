using UnityEngine;
using System.Linq;
using System;

namespace Scripts.OOP.Game_Modes.Story
{
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

    public abstract class Chapter1 : StoryMode
    { 
        public Chapter1(StoryLevel levelSettings, MainMenuHandler menu,
            MapHandler map, int extra_teams = 0) : base(levelSettings, menu, map, 
                ChapterSettings.GetTeams(extra_teams) ) { }
    }
}
