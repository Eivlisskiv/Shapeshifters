using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.OOP.UI
{
    public abstract class GameModeUI
    {
        private Text score;

        public void SetScore(int score)
            => this.score.text = score.ToString();

        private void InitButton(MainMenuHandler menu, Transform child)
        {
            GeneralButton button = child.GetComponent<GeneralButton>();
            button.OnPress.AddListener(GetOnClick(menu));
        }

        protected abstract UnityAction GetOnClick(MainMenuHandler menu);

        private void InitDescription(Transform transform)
        {
            Text desc = transform.GetChild(0).GetComponent<Text>();
            desc.text = GetDescription();
        }

        protected abstract string GetDescription();

        protected virtual void BeforeInit() { }

        public void InitializeUI(GameObject ui, MainMenuHandler menu)
        {
            InitScore(ui.transform.GetChild(0));
            InitTitle(ui.transform.GetChild(1));
            InitButton(menu, ui.transform.GetChild(2));
            InitDescription(ui.transform.GetChild(3));
        }

        protected virtual void AfterInit() { }

        private void InitScore(Transform transform)
        {
            Transform p = transform.GetChild(1);
            score = p.GetComponent<Text>();
            score.text = GetTopScore().ToString();
        }

        protected abstract int GetTopScore();

        private void InitTitle(Transform child)
        {
            Text t = child.GetComponent<Text>();
            t.text = GetName();
        }

        protected abstract string GetName();
    }
}