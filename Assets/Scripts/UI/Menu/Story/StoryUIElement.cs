using Scripts.OOP.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.UI.Menu.Story
{
    public class StoryUIElement : GameModeUI
    {
        

        public StoryUIElement(int chapter, int episode, GameObject ui, MainMenuHandler menu) 
            : base(ui, menu) { }

        protected override string GetDescription()
        {
            throw new NotImplementedException();
        }

        protected override string GetName()
        {
            throw new NotImplementedException();
        }

        protected override UnityAction GetOnClick(MainMenuHandler menu)
        {
            throw new NotImplementedException();
        }

        protected override int GetTopScore()
        {
            throw new NotImplementedException();
        }
    }
}
