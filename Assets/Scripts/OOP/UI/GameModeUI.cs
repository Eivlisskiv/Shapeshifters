using Scripts.OOP.Game_Modes;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.UI
{
    public class GameModeUI
    {
        public readonly Type mode;
        readonly MainMenuHandler menu;
        internal Text score;

        public GameModeUI(Type mode, string description, GameObject ui, MainMenuHandler menu)
        {
            this.menu = menu;
            this.mode = mode;
            InitializeUI(ui, description);
        }

        public void SetScore(int score)
            => this.score.text = score.ToString();

        private void InitializeUI(GameObject ui, string desc)
        {
            //main = ui;
            InitScore(ui.transform.GetChild(0));
            InitDescription(ui.transform.GetChild(1), desc);
            InitTitle(ui.transform.GetChild(2));
            InitButton(ui.transform.GetChild(3));
        }

        private void InitTitle(Transform child)
        {
            Text t = child.GetComponent<Text>();
            t.text = mode.Name.Replace('_', ' ');
        }

        private void InitButton(Transform child)
        {
            Button button = child.GetComponent<Button>();
            button.onClick.AddListener(() => menu.StartGame(mode));
        }

        private void InitScore(Transform transform)
        {
            Transform p = transform.GetChild(1);
            score = p.GetComponent<Text>();
            score.text = GameModes.LoadScore(mode).ToString();
        }

        private void InitDescription(Transform transform, string description)
        {
            Text desc = transform.GetChild(0).GetComponent<Text>();
            desc.text = description;
        }
    }
}
