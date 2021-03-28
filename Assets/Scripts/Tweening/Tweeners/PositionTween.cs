using System;
using UnityEngine;

namespace Scripts.Tweening.Tweeners
{
    class PositionTween : Tweener
    {
        public PositionTween(Transform element, Vector3 target, float time, float delay)
            : base(element, target, time, delay) { }

        protected override Vector3 GetStart()
            => element.localPosition;

        protected override void OnFinish()
            => element.localPosition = target;

        protected override void OnMove(Vector3 current)
            => element.localPosition = current;
    }
}
