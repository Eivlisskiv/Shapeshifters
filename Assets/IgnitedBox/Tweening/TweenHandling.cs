using IgnitedBox.Tweening.Components;
using IgnitedBox.Tweening.Tweeners;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using System;
using UnityEngine;

namespace IgnitedBox.Tweening
{
    public static class TweenHandling
    {
        private static GameObject _tweenHandler;
        public static GameObject TweenHandler => 
            _tweenHandler ? _tweenHandler :
            (_tweenHandler = new GameObject("Tween Handler", 
                typeof(GlobalTweensContainer)));

        private static GlobalTweensContainer _conatiner;

        public static GlobalTweensContainer Conatiner
            => _conatiner ? _conatiner : (_conatiner =
            TweenHandler.GetComponent<GlobalTweensContainer>());

        private static A Construct<T, V, A>(T subject, V target,
            float time, float delay = 0, Func<double, double> easing = null,
            Action callback = null) where A : TweenData<T, V>
            => (A)Activator.CreateInstance(typeof(A),
                subject, target, time, delay, easing, callback);

        public static TTweener Tween<TElement, TValue, TTweener>(this TElement subject, TValue target, 
            float time, float delay = 0, Func<double, double> easing = null,
            Action callback = null) where TTweener : TweenData<TElement, TValue>
        {
            TTweener tween = Construct<TElement, TValue, TTweener>(subject, target, time,
                delay, easing, callback);

            if (subject is Component component)
            return AttachComponentTween<TTweener, TElement, TValue>(component.gameObject,
                tween, Tweener.BlendType.Replace);
            
            Conatiner.Add(tween);

            return tween;
        }

        public static A Tween<T, V, A>(this T subject, V target, Tweener.BlendType blend,
            float time, float delay = 0, Func<double, double> easing = null,
            Action callback = null) where A : TweenData<T, V> where T : Component
        {
            A tween = Construct<T, V, A>(subject, target, time, delay, easing, callback);
            return AttachComponentTween<A, T, V>(subject.gameObject, tween, blend);
        }

        private static A AttachComponentTween<A, T, V>
            (GameObject gameObject, A tween, Tweener.BlendType blend) where A : TweenData<T, V>
        {
            if (!gameObject) return null;

            Tweener tweenComponent = gameObject.GetComponent<Tweener>();
            if (!tweenComponent) tweenComponent = gameObject.AddComponent<Tweener>();

            tweenComponent.Add<A, T, V>(tween, blend);
            return tween;
        }
    }
}
