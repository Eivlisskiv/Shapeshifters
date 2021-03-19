using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Audio
{
    [System.Serializable]
    public class HashedClips : ClipsCollection
    {
        private Dictionary<string, AudioEntity> hashed;

        public override void Initialize()
        {
            hashed = new Dictionary<string, AudioEntity>();
            for (int i = 0; i < clips.Length; i++)
            {
                AudioEntity clip = clips[i];
                if (clip.clip) hashed.Add(clip.clip.name, clip);
            }
        }

        public void Play(string name, GameObject gameObject)
        {
            if (!hashed.TryGetValue(name, out AudioEntity ae))
            {
                Debug.LogWarning(
                    $"{name} is not in the audio clips of {gameObject.name}");
                return;
            }

            ae.Play(gameObject);
        }
    }
}
