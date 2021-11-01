using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners
{
    public abstract class TweenData<S, T> : TweenerBase
    {
        [SerializeField]
        private S _element;
        public S Element => _element;

        [SerializeField]
        private T _start;
        public T Start 
        {
            get => _start;
            set
            {
                _start = value;
                _tween = GetTween();
            }
        }

        private T _tween;
        public T Tween  => _tween;

        [SerializeField]
        private T _target;
        public T Target 
        {
            get => _target;
            set
            {
                _target = value;
                _tween = GetTween();
            } 
        }

        protected TweenData() { }

        protected TweenData(S subject, T target, float time, 
            float delay, Func<double, double> easing, Action callback)
        {
            _element = subject;
            _target = target;

            Start = GetStart();
            Duration = time;
            Delay = delay;

            Easing = easing;
            Callback = callback;
        }

        public abstract void Blend(TweenData<S, T> with);

        public abstract T GetTweenAt(float percent);

        protected abstract T GetStart();
        protected abstract T GetTween();
        protected abstract void OnFinish();
        protected abstract void OnMove(T current);

        public override void Update(float time)
        {
            if (Element == null) return;

            if (!ContinueDelay(time)) return;

            if(Check(time, out float percent))
            {
                OnFinish();

                Callback?.Invoke();
                callbackEvent?.Invoke();

                if (DoLoop()) return;
                State = TweenState.Finished;
                return;
            }

            OnMove(GetTweenAt(percent));
            return;
        }

        public void SetPositions(T start, T target)
        {
            _start = start;
            _target = target;
            _tween = GetTween();
        }

        private bool DoLoop()
        {
            switch (loop)
            {
                case LoopType.None: return false;

                case LoopType.ReverseLoop:
                    SetPositions(Target, Start);
                    break;
            }

            currentDelay = Delay; Time = 0;

            return true;
        }

#if UNITY_EDITOR
        public override void EditorObjectField()
        {
            Type type = typeof(S);
            if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                var obj = UnityEditor.EditorGUILayout.ObjectField(type.Name,
                    Element as UnityEngine.Object, type, true);
                if (obj is S s) _element = s;
                return;
            }

            base.EditorObjectField();
        }
#endif
    }
}
