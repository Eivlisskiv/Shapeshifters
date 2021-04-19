using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using UnityEngine;

namespace IgnitedBox.Tweening.Conponents.Transforms
{
    public class RectTransformTween : TransformTween
    {
        public RectSizeTween size;

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (size != null && size.Element) size.Update(Time.deltaTime);
        }
    }
}
