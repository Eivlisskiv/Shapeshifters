using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.ColorTweeners
{
    public abstract class ColorTweener<T> : TweenData<T, Color>
    {
        protected ColorTweener() { }

        protected ColorTweener(T element, Color target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        public override void Blend(TweenData<T, Color> with)
            => Target *= with.Target;

        public override Color GetTweenAt(float percent)
            => Start + (Tween * percent);

        protected override Color GetTween()
            => Target - Start;

#if UNITY_EDITOR
        public override void EditorValueFields()
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            Start = UnityEditor.EditorGUILayout.ColorField("Start", Start);
            if (GUILayout.Button("Current", GUILayout.Width(55)) && Element != null)
                Start = GetStart();
            UnityEditor.EditorGUILayout.EndHorizontal();

            UnityEditor.EditorGUILayout.BeginHorizontal();
            Target = UnityEditor.EditorGUILayout.ColorField("Target", Target);
            if (GUILayout.Button("Current", GUILayout.Width(55)) && Element != null)
                Target = GetStart();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }
#endif
    }
}
