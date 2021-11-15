using UnityEngine;

namespace Scripts.Explosion
{
    public class FireworkExplosionHandler : ExplosionHandler
    {
        public ParticleSystem secondary;

        private ParticleSystem Secondary
        {
            get
            {
                if (!secondary)
                    secondary = MainEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
                return secondary;
            }
        }

        protected override void SetIntensity(float value)
        {
            var burst = Secondary.emission.GetBurst(0);
            burst.count = Mathf.Max((int)value, 5);
            Secondary.emission.SetBurst(0, burst);
        }

        protected override ParticleSystem SetDuration()
        {
            FullStop();

            var mmain = MainEffect.main;
            mmain.duration = Duration * 1.2f;
            mmain.startLifetime = Duration * 1.2f;

            var sub = Secondary;
            var main = sub.main;
            main.duration = Duration;
            main.startLifetime = Duration;
            return sub;
        }

        protected override float GetRotation()
        {
            var shape = Secondary.shape;
            return shape.rotation.z;
        }

        protected override void SetRotation(float value)
        {
            var shape = Secondary.shape;
            shape.rotation = new Vector3(0, 0, value);
        }

        protected override void SetAngle(float value)
        {
            var shape = Secondary.shape;
            shape.arc = value;
        }

        protected override float GetAngle()
        {
            var shape = Secondary.shape;
            return shape.arc;
        }
    }
}
