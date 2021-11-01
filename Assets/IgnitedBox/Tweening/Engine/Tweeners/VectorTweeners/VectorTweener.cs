using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners.VectorTweeners
{
    public abstract class VectorTweener<T> : TweenData<T, Vector3>
    {
        protected VectorTweener() { }

        protected VectorTweener(T element, Vector3 target, float time,
            float delay, Func<double, double> easing, Action callback)
            : base(element, target, time, delay, easing, callback) { }
        
        protected override Vector3 GetTween()
            => Target - Start;

        public override Vector3 GetTweenAt(float percent)
            => Start + (Tween * percent);

#if UNITY_EDITOR
        public override void EditorValueFields()
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            Start = UnityEditor.EditorGUILayout.Vector3Field("Start", Start);
            if(GUILayout.Button("Current", GUILayout.Width(55)) && Element != null)
                Start = GetStart();
            UnityEditor.EditorGUILayout.EndHorizontal();

            UnityEditor.EditorGUILayout.BeginHorizontal();
            Target = UnityEditor.EditorGUILayout.Vector3Field("Target", Target);
            if (GUILayout.Button("Current", GUILayout.Width(55)) && Element != null)
                Target = GetStart();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }
#endif
    }
}
