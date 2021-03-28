using Scripts.Tweening.Tweeners;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UITween : MonoBehaviour
{
    List<Tweener> tweens;

    private void Start()
    {
        tweens = new List<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tweens.Count == 0) return;
        int i = 0;
        while (i < tweens.Count)
        {
            Tweener tween = tweens[i];
            if (tween.Update(Time.deltaTime)) tweens.RemoveAt(i);
            else i++;
        }
    }

    public Tweener Tween<T>(RectTransform transform, 
        Vector3 target, float time, float delay = 0) where T : Tweener
    {
        Tweener tween = (Tweener)Activator.CreateInstance(typeof(T), 
            transform, target, time, delay);
        tweens.Add(tween);
        return tween;
    }
}
