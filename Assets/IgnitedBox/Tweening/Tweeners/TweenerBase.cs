using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners
{
    [Serializable]
    public abstract class TweenerBase
    {
        public enum TweenState
        {
            Playing, Paused, Finished
        }

        public enum LoopType 
        { 
            None, ResetLoop, ReverseLoop
        }

        public TweenState State 
        {
            get;
            protected set;
        }

        [SerializeField]
        private float _delay;
        public float Delay 
        {
            get => _delay;
            set
            {
                _delay = value;
                currentDelay = Math.Min(currentDelay, _delay);
            } 
        }

        [SerializeField]
        protected float currentDelay;

        [SerializeField]
        public float Duration { get; set; }

        [SerializeField]
        public float Time { get; set; }

        [SerializeField]
        private AnimationCurve _curve;
        public AnimationCurve Curve
        {
            get => _curve;
            set
            {
                _curve = value;
                if (_curve != null)
                    Easing = (t) => _curve.Evaluate((float)t);
                else Easing = null;
            }
        }

        public Func<double, double> Easing { get; set; }

        public Action Callback { get; set; }

        [SerializeField]
        public LoopType loop;

        public TweenerBase() { }

#if UNITY_EDITOR
        public bool editorOpen;

        public virtual void EditorObjectField() 
            => UnityEditor.EditorGUILayout.LabelField("Element Field not implemented!");
        public virtual void EditorValueFields()
            => UnityEditor.EditorGUILayout.LabelField("Values Fields not implemented!");
#endif

        public virtual bool Update(float time) 
            =>  State == TweenState.Playing && (currentDelay < 0 || (currentDelay -= time) < 0);

        protected bool Check(float time, out float percent)
        {
            Time += time;
            float x = Time / Duration;
            percent = Easing == null ? x : (float)Easing(x);
            return x >= 1;
        }

        public void SetNormalizedTime(double normalizedTime)
            => Time = (float)(Duration * normalizedTime);
    }
}
