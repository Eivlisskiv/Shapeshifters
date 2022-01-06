using IgnitedBox.Tweening.Tweeners;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IgnitedBox.Tweening.Components
{
    [Serializable]
    public class Tweener : MonoBehaviour
    {
        public enum BlendType { Replace, Blend, Ignore }

        public int Count => tweens?.Count ?? 0;

        [SerializeField]
        private List<TweenerBase> tweens
            = new List<TweenerBase>();

        void Update() => TweenTransforms();

        private void TweenTransforms()
        {
            if (Count == 0) return;
            int i = 0;
            while (i < tweens.Count)
            {
                tweens[i].Update(tweens[i].scaledTime ? Time.deltaTime : Time.unscaledDeltaTime);
                if (tweens[i].State == TweenerBase.TweenState.Finished) 
                    tweens.RemoveAt(i);
                else i++;
            }
        }

        public TweenerBase Get(int i) => tweens[i];
        public T Get<T>(out int index) where T : TweenerBase
        {
            index = tweens.FindIndex(t => t is T);
            if (index > -1 && tweens[index] != null && tweens[index] is T tween) return tween;
            return default;
        }

        public A Add<A, S, T>(A tween, BlendType blend) where A : TweenData<S, T>
        {
            Type type = typeof(A);
            int i = tweens.FindIndex(t => t is A);
            if(i >= 0)
            {
                A current = (A)tweens[i];
                switch (blend)
                {
                    case BlendType.Replace:
                        tweens[i] = tween;
                        return tween;
                    case BlendType.Blend:
                        current.Blend(tween);
                        return current;
                    default:
                        return current;
                }
            }

            tweens.Add(tween);
            return tween;
        }

        public bool ContainsType(Type type)
            => tweens.Find(t => t.GetType().Equals(type)) != null;

        public void Add(Type type)
        {
            if (ContainsType(type)) return;
            TweenerBase b = (TweenerBase)Activator.CreateInstance(type);
            tweens.Add(b);
        }

        public void Remove(TweenerBase element)
            => tweens.Remove(element);

        public void Remove(int i) => tweens.RemoveAt(i);

        public void Remove(Type key)
            => tweens.RemoveAll(t => t.GetType().Equals(key));
    }
}
