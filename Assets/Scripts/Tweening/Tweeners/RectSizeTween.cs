using UnityEngine;

namespace Scripts.Tweening.Tweeners
{
    class RectSizeTween : Tweener
    {
        public RectSizeTween(Transform element, Vector3 target, float time, float delay)
            : base(element, target, time, delay) { }

        protected override Vector3 GetStart()
            => ((RectTransform)element).sizeDelta;

        protected override void OnFinish()
            => ((RectTransform)element).sizeDelta = target;

        protected override void OnMove(Vector3 current)
            => ((RectTransform)element).sizeDelta = current;
    }
}
