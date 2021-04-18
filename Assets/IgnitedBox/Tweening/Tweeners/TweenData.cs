using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners
{
    public abstract class TweenData<S, T> : TweenerBase
    {
        [SerializeField]
        public S Subject { get; private set; }

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

        [SerializeField]
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
            Subject = subject;
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

        public override bool Update(float time)
        {
            if (Subject == null) return true;

            if (!base.Update(time)) return false;

            if(Check(time, out float percent))
            {
                OnFinish();
                Callback?.Invoke();
                if (DoLoop()) return false;
                State = TweenState.Finished;
                return true;
            }

            OnMove(GetTweenAt(percent));
            return false;
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
                    Subject as UnityEngine.Object, type, true);
                if (obj is S s) Subject = s;
                return;
            }

            base.EditorObjectField();
        }
#endif
    }
}
