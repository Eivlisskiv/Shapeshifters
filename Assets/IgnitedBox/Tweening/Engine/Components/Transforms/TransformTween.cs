using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using UnityEngine;

namespace IgnitedBox.Tweening.Components.Transforms
{
    public class TransformTween : MonoBehaviour
    {
        public PositionTween position;
        public ScaleTween scale;

        // Update is called once per frame
        void Update() => OnUpdate();

        protected virtual void OnUpdate()
        {
            if (position != null && position.Element) position.Update(Time.deltaTime);
            if (scale != null && scale.Element) scale.Update(Time.deltaTime);
        }
    }
}
