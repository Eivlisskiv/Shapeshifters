using Scripts.OOP.MongoRealm;
using System;
using UnityEngine.Events;

namespace Scripts.OOP.UI
{
    public class ArcadeModeUI : GameModeUI
    {
        public readonly Type mode;
        public readonly string desc;
        private string name;

        public ArcadeModeUI(Type mode, string description)
        {
            this.mode = mode;
            desc = description;
        }

        protected override string GetDescription() => desc;

        protected override string GetName()
        {
            if(name == null) name = mode.Name.Replace('_', ' ');
            return name;
        }

        protected override UnityAction GetOnClick(MainMenuHandler menu)
            => () => menu.StartGame(this);

        protected override int GetTopScore()
        {
            string id = GetName();
            ArcadeProgress prog = ArcadeProgress.LoadOne<ArcadeProgress>(id, true);
            return prog.TopScore;
        }
    }
}
