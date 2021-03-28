using UnityEngine;

namespace Scripts.Tweening.Tweeners
{
    public abstract class Tweener
    {
        protected readonly Transform element;

        protected readonly Vector3 start;
        protected readonly Vector3 vector;
        protected readonly Vector3 target;

        protected float delay;

        protected readonly float totalTime;
        protected float time;


        protected Tweener(Transform element, Vector3 target, float time, float delay)
        {
            this.element = element;
            this.target = target;
            start = GetStart();
            vector = target - start;
            totalTime = time;
            this.delay = delay;
        }

        protected abstract Vector3 GetStart();

        protected abstract void OnFinish();
        protected abstract void OnMove(Vector3 current);

        public bool Update(float time)
        {
            if (delay > 0 && (delay - time) > 0) return false;

            if(Check(time, out float percent))
            {
                OnFinish();
                return true;
            }

            OnMove(start + (vector * percent));
            return false;
        }

        public bool Check(float time, out float percent)
        {
            this.time += time;
            percent = this.time / totalTime;
            return percent >= 1;
        }
    }
}
