using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.Audio
{
    [System.Serializable]
    public class RandomClips : ClipsCollection
    {
        public void PlayRandom(GameObject obj)
        {
            if(clips.Length == 0)
            {
                Debug.LogWarning($"RandomCLips {name} in {obj.name} has no clips");
                return;
            }

            AudioEntity ae = clips.Length == 1 ? clips[0] : Randomf.Element(clips);
            if (!ae.clip)
            {
                Debug.LogWarning($"Random AudioEntity in {obj.name} is missing a clip");
                return;
            }

            ae.Play(obj);
        }
    }
}
