using UnityEngine;

namespace Scripts.OOP.Audio
{
    [System.Serializable]
    public class AudioEntity
    {
        public enum ReplaySetting
        {
            None, Restart, Delay
        }

        public AudioClip clip;

        public ReplaySetting replaySetting;

        [Range(0f, 1f)]
        public float volume = 1;

        [Range(0f, 1f)]
        public float spacialBlend = 0;

        private AudioSource source;

        public void SetSource(AudioSource source)
        {
            this.source = source;
            source.clip = clip;

            source.volume = volume;
            source.spatialBlend = spacialBlend;
        }

        private void CreateSource(GameObject obj)
            => SetSource(obj.AddComponent<AudioSource>());
            

        public void Play(GameObject obj)
        {
            if (!source) CreateSource(obj);
            else if (source.isPlaying)
            {
                Replay();
                return;
            }
            Play();
        }

        private void Replay()
        {
            switch (replaySetting)
            {
                case ReplaySetting.Delay:
                    source.PlayDelayed(clip.length - source.time);
                    break;
                case ReplaySetting.Restart:
                    source.Stop();
                    Play();
                    break;
            }
        }

        private void Play()
        {
            source.Play();
        }
    }
}

