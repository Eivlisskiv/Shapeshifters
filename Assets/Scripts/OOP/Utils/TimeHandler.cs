using UnityEngine;

namespace Scripts.OOP.Utils
{
    class TimeHandler
    {
        public static TimeHandler Instance 
        { get => _instance; }

        private static readonly TimeHandler _instance = new TimeHandler();

        private readonly float defaultFixed;
        private readonly float defaultScale;

        private float gameFixed;
        private float gameScale;

        private TimeHandler()
        {
            defaultFixed = Time.fixedDeltaTime;
            gameFixed = defaultFixed;
            defaultScale = Time.timeScale;
            gameScale = defaultScale;
        }

        private float SetFixed(float t)
        {
            gameFixed = t;
            Time.fixedDeltaTime = t;
            return t;
        }

        private float SetScale(float t)
        {
            gameScale = t;
            Time.timeScale = t;
            return t;
        }

        private void SetFixedDelta(float scale)
            => SetFixed(defaultFixed * scale);

        public void Reset()
        {
            SetScale(defaultScale);
            SetFixed(defaultFixed);
        }

        public void SetTimeScale(float scale)
            => SetFixedDelta(SetScale(scale));

        public bool LerpScale(float target, float lerp)
        {
            float current = Time.timeScale;
            bool finish = Mathf.Abs(target - current) < 0.05;
            float scale = finish ? target : Mathf.Lerp(current, target, lerp);
            SetTimeScale(scale);
            return finish;
        }

        public void Pause()
        {
            Time.fixedDeltaTime = 0;
            Time.timeScale = 0;
        }

        public void Resume()
        {
            Time.fixedDeltaTime = gameFixed;
            Time.timeScale = gameScale;
        }
    }
}
