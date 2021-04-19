using IgnitedBox.Tweening.Conponents.Transforms;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using UnityEngine;

namespace IgnitedBox.Tweening.Conponents.UI
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
