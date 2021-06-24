using Scripts.OOP.Audio;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public RandomClips[] alts;
    public HashedClips[] sounds;

    private Dictionary<string, RandomClips> randoms;
    private Dictionary<string, HashedClips> hashed;

    // Start is called before the first frame update
    void Awake()
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

    public void UpdateClips<T>(T collection)
        where T : ClipsCollection
    {
        collection.Initialize();

        if (UpdateDict(collection, randoms)) return;

        if (UpdateDict(collection, hashed)) return;

    }

    private bool UpdateDict<T, D>(T collection, Dictionary<string, D> dict) 
        where T : ClipsCollection
        where D : ClipsCollection
    {
        if (typeof(T) == typeof(D))
        {
            dict[collection.name] = collection as D;
            return true;
        }

        return false;
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
