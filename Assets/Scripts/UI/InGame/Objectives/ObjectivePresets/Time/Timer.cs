using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Time
{
    public class Timer : ObjectivePreset, IOnUpdate
    {
        protected float TimeLeft { get; private set; }

        protected Text TimerText { get; private set; }

        public Timer(GameObject element, ObjectiveData data = null) 
            : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            Game.ObjectiveEvents.Subscribe<CustomLevel, float>
                (typeof(IOnUpdate), Progress);

            TimeLeft = LoadParam<float>(data, 1, 10);

            Text title = base.Initialize(data);

            TimerText = Get<Text>("Timer", t =>
            {
                t.text = TimeString();
                t.alignment = TextAnchor.MiddleCenter;
            });

            return title;
        }

        public void Progress(CustomLevel game, float time)
        {
            if (IsSpawning) return;

            TimeLeft -= time;
            if(TimeLeft > 0)
            {
                TimerText.text = TimeString();
                return;
            }

            Completed();
        }

        private string TimeString()
        {
            float seconds = TimeLeft;
            int minutes;
            int hours = (int)(seconds / 3600);

            seconds -= hours * 3600;
            minutes = (int)(seconds / 60);
            seconds = Mathf.Round((seconds - (minutes * 60)) * 100) / 100;

            string hs = hours <= 0 ? null :
                (hours < 10 ? $"0{hours}:" : $"{hours}:");

            string ms = minutes <= 0 ? null :
                (minutes < 10 ? $"0{minutes}:" : $"{minutes}:");

            string ss = seconds < 10 ? $"0{(int)seconds}" : $"{(int)seconds}";

            return hs + ms + ss;
        }
    }
}
