using IgnitedBox.Tweening.Components.Transforms;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using UnityEngine;

namespace IgnitedBox.Tweening.Components.UI
{
    public class UITween : TransformTween
    {
        public GraphicColorTween color;

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (color != null && color.Element) color.Update(Time.deltaTime);
        }
    }
}
