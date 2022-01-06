using IgnitedBox.Tweening.Components;
using System;
using UnityEngine;

namespace IgnitedBox.Tweening.Tweeners
{
    public abstract class TweenData<TElement, TTween> : TweenerBase
    {
        [SerializeField]
        private TElement _element;
        public TElement Element => _element;

        [SerializeField]
        private TTween _start;
        public TTween Start 
        {
            get => _start;
            set
            {
                _start = value;
                _tween = GetTween();
            }
        }

        private TTween _tween;
        public TTween Tween  => _tween;

        [SerializeField]
        private TTween _target;
        public TTween Target 
        {
            get => _target;
            set
            {
                _target = value;
                _tween = GetTween();
            } 
        }

        protected TweenData() { }

        protected TweenData(TElement subject, TTween target, float time, 
            float delay, Func<double, double> easing, Action callback)
        {
            _element = subject;
            _target = target;

            Start = GetStart();
            Duration = time;
            Delay = delay;
            currentDelay = Delay;

            Easing = easing;
            Callback = callback;
        }

        public abstract void Blend(TweenData<TElement, TTween> with);

        public abstract TTween GetTweenAt(float progress);

        protected abstract TTween GetStart();
        protected abstract TTween GetTween();
        protected abstract void OnMove(TTween current);

        public override void Update(float time)
        {
            if (Element == null) return;

            if (!ContinueDelay(time)) return;

            if(Check(time, out float percent))
            {
                OnMove(GetTweenAt(PerformEasing(1)));

                Callback?.Invoke();
                callbackEvent?.Invoke();

                if (DoLoop()) return;
                State = TweenState.Finished;
                return;
            }

            OnMove(GetTweenAt(percent));
            return;
        }

        public void SetPositions(TTween start, TTween target)
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

        public void Dispose()
        {
            Stop();
            if(Element is Component unityComponent)
            {
                //Component is not available
                if (!unityComponent) return;

                Tweener tweenComponent = unityComponent.GetComponent<Tweener>();

                tweenComponent.Remove(this);

                return;
            }

            TweenHandling.Container.Dispose(this);
        }

#if UNITY_EDITOR
        public override void EditorObjectField()
        {
            Type type = typeof(TElement);
            if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                var obj = UnityEditor.EditorGUILayout.ObjectField(type.Name,
                    Element as UnityEngine.Object, type, true);
                if (obj is TElement s) _element = s;
                return;
            }

            base.EditorObjectField();
        }
#endif
    }
}
