using IgnitedBox.Tweening.Tweeners;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IgnitedBox.Tweening.Conponents
{
    public class Tweener : MonoBehaviour
    {
        public enum BlendType { Replace, Blend, Ignore }

        [SerializeField]
        public Dictionary<Type, TweenerBase> tweens
            = new Dictionary<Type, TweenerBase>();

        void Update()
            => TweenTransforms();

        private void TweenTransforms()
        {
            if (tweens.Count == 0) return;
            int i = 0;
            while (i < tweens.Count)
            {
                KeyValuePair<Type, TweenerBase> pair = tweens.ElementAt(i);
                if (pair.Value.Update(Time.deltaTime)) 
                    tweens.Remove(pair.Key);
                else i++;
            }
        }

        public A Add<A, S, T>(A tween, BlendType blend) where A : TweenData<S, T>
        {
            Type type = typeof(A); 
            if(tweens.TryGetValue(type, out TweenerBase cbase))
            {
                A current = (A)cbase;
                switch (blend)
                {
                    case BlendType.Replace:
                        tweens[type] = tween;
                        return tween;
                    case BlendType.Blend:
                        current.Blend(tween);
                        return current;
                    default:
                        return current;
                }
            }

            tweens.Add(type, tween);
            return tween;
        }

        public void Add(Type type)
        {
            if (tweens.ContainsKey(type)) return;
            TweenerBase b = (TweenerBase)Activator.CreateInstance(type);
            tweens.Add(type, b);
        }

        public void Remove(Type key)
            => tweens.Remove(key);
    }
}
