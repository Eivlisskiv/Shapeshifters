using Scripts.OOP.Audio;
using UnityEngine;

public class AwakeAudio : MonoBehaviour
{
    public RandomClips clips;

    private void Awake()
    {
        clips.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        clips.PlayRandom(gameObject);
    }
}
