using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.FloatTweeners
{
    public abstract class FloatTweener<T> : TweenData<T, float>
    {
        protected FloatTweener() { }

        protected FloatTweener(T element, float target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }

        protected override float GetTween()
            => Target - Start;
        public override float GetTweenAt(float percent)
            => Start + (Tween * percent);

#if UNITY_EDITOR
        public override void EditorValueFields()
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            Start = UnityEditor.EditorGUILayout.FloatField("Start", Start);
            if (GUILayout.Button("Current", GUILayout.Width(55)) && Element != null)
                Start = GetStart();
            UnityEditor.EditorGUILayout.EndHorizontal();

            UnityEditor.EditorGUILayout.BeginHorizontal();
            Target = UnityEditor.EditorGUILayout.FloatField("Target", Target);
            if (GUILayout.Button("Current", GUILayout.Width(55)) && Element != null)
                Target = GetStart();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }
#endif
    }
}
