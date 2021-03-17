using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.UI
{
    public class StatBar
    {
        readonly Image live;
        readonly Image delayed;

        float target;
        bool healing;

        bool done;

        public StatBar(Transform container)
        {
            live = container.Find("Live").GetComponent<Image>();
            delayed = container.Find("Delay").GetComponent<Image>();
        }

        public void Update(float v)
        {
            if (v != target)
            {
                healing = v > target;
                target = v;
                done = false;
            }
            else if (done) return;

            if (healing) UpdateDirection(delayed, live);
            else UpdateDirection(live, delayed);
        }

        private void UpdateDirection(Image to, Image from)
        {
            float tov = to.fillAmount;
            if(tov != target) to.fillAmount = target;
            float fromv = from.fillAmount;
            if (fromv != target)
            {
                if (Mathf.Abs(target - fromv) < 0.01)
                {
                    from.fillAmount = target;
                    done = true;
                }
                else from.fillAmount = Mathf.Lerp(fromv, target, Time.deltaTime);
            }
        }
    }
}
