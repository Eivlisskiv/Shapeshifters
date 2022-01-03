using UnityEngine;

namespace Scripts.OOP.Audio
{
    public class VolumeSetting
    {
        internal static readonly VolumeSetting Effect = new VolumeSetting("Effect");
        internal static readonly VolumeSetting Music = new VolumeSetting("MusicEffect");

        private readonly string name;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                PlayerPrefs.SetFloat(name, _volume);
            }
        }

        private float _volume;

        public VolumeSetting(string name)
        {
            this.name = $"Volume_{name}";

            _volume = PlayerPrefs.GetFloat(this.name, 1);
        }
    }
}
