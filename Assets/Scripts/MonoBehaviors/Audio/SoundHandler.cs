using Scripts.OOP.Audio;
using Scripts.OOP.Utils;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public RandomClips[] alts;
    public HashedClips[] sounds;

    private Dictionary<string, RandomClips> randoms;
    private Dictionary<string, HashedClips> hashed;

    // Start is called before the first frame update
    void Start()
    {
        randoms = HashClipLists(alts);
        hashed = HashClipLists(sounds);
    }

    private Dictionary<string, T> HashClipLists<T>(T[] collections) 
        where T : ClipsCollection
    {
        Dictionary<string, T> dict = new Dictionary<string, T>();
        for (int i = 0; i < collections.Length; i++)
        {
            T clips = collections[i];
            if (clips.name != null)
            {
                clips.Initialize();
                dict.Add(clips.name, clips);
            }
        }

        return dict;
    }

    public void PlayRandom(string category)
    {
        if(randoms == null || !randoms.TryGetValue(category, out RandomClips clips))
        {
            Debug.LogWarning(
                $"There are no clips collection named {category} in {gameObject.name}");
            return;
        }

        clips.PlayRandom(gameObject);
    }

    public void Play(string category, string name)
    {
        if (hashed == null || !hashed.TryGetValue(category, out HashedClips clips))
        {
            Debug.LogWarning(
                $"There are no clips collection named {category} in {gameObject.name}");
            return;
        }

        clips.Play(name, gameObject);
    }
}
