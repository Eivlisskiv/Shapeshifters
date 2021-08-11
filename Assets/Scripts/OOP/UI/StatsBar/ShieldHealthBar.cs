using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.UI.StatsBar
{
    public class ShieldHealthBar : StatusBar
    {
        readonly Image shield;

        public ShieldHealthBar(Transform container) : base(container)
        {
            shield = container.Find("Shields").GetComponent<Image>();
        }

        public void SetShield(float percent)
            => TweenFill(shield, percent, 1);
    }
}
