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

        private TimeHandler()
        {
            defaultFixed = Time.fixedDeltaTime;
            defaultScale = Time.timeScale;
        }

        private void SetFixedDelta(float scale)
            => Time.fixedDeltaTime = defaultFixed * scale;

        public void Reset()
        {
            Time.timeScale = defaultScale;
            Time.fixedDeltaTime = defaultFixed;
        }

        public void SetScale(float scale)
            => SetFixedDelta((Time.timeScale = scale));

        public bool LerpScale(float target, float lerp)
        {
            float current = Time.timeScale;
            bool finish = Mathf.Abs(target - current) < 0.05;
            float scale = finish ? target : Mathf.Lerp(Time.timeScale, target, lerp);
            Time.timeScale = scale;
            SetFixedDelta(scale);
            return finish;
        }
    }
}
