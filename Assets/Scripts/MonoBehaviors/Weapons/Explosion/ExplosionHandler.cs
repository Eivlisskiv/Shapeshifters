using System;
using UnityEngine;

namespace Scripts.Explosion
{
    [Serializable]
    public class ExplosionHandler : MonoBehaviour
    {
        public ParticleSystem mainExplosion;

        public float Intensity
        {
            get => _intensity;
            set
            {
                _intensity = value;
                SetIntensity(value);
            }
        }

        [SerializeField]
        [HideInInspector]
        private float _intensity;

        public float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                SetSpeed(value);
            }
        }

        [SerializeField]
        [HideInInspector]
        private float _speed;

        public float Range
        {
            get => _range;
            set
            {
                _range = value;
                SetRange(value);
            }
        }

        [SerializeField]
        [HideInInspector]
        private float _range;

        public float Duration => Range / Speed;

        public float Rotation
        {
            get => GetRotation();
            set => SetRotation(value);
        }

        public float Angle
        {
            get => GetAngle();
            set => SetAngle(value);
        }

        public virtual void FullStop()
        {
            if (!MainEffect.isPlaying) return;
            MainEffect.Stop(true,
                ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public virtual void Play()
        {
            if (!MainEffect || MainEffect.isPlaying) return;

            MainEffect.Play();
        }

        public virtual void Restart()
        {
            if (MainEffect.isPlaying) MainEffect.Stop();
            MainEffect.Play();
        }

        protected ParticleSystem MainEffect
        {
            get
            {
                if (!mainExplosion)
                {
                    mainExplosion = GetComponent<ParticleSystem>();
                }
                return mainExplosion;
            }
        }

        protected virtual void SetIntensity(float value)
        {
            var burst = MainEffect.emission.GetBurst(0);
            burst.count = (int)value;
            MainEffect.emission.SetBurst(0, burst);
        }

        protected virtual void SetSpeed(float value)
        {
            var main = SetDuration().main;
            main.startSpeed = new ParticleSystem.MinMaxCurve
                (value * 0.9f, value * 1.1f);
        }

        protected virtual void SetRange(float value)
        {
            var main = MainEffect.main;
            main.startSize = value / 2.5f;
            SetDuration();
        }

        protected virtual ParticleSystem SetDuration()
        {
            FullStop();

            var mmain = MainEffect.main;
            mmain.duration = Duration * 1.2f;
            mmain.startLifetime = Duration * 1.2f;

            var sub = GetSub(MainEffect);
            var main = sub.main;
            main.duration = Duration;
            main.startLifetime = Duration;
            return sub;
        }

        protected virtual void SetRotation(float value)
        {
            var current = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(current.x, current.y, value);
        }

        protected virtual float GetRotation()
        {
            return transform.localRotation.eulerAngles.z;
        }

        protected virtual void SetAngle(float value)
        {
            var shape = GetSub(MainEffect).shape;
            shape.arc = value;
        }

        protected virtual float GetAngle()
        {
            var shape = GetSub(MainEffect).shape;
            return shape.arc;
        }

        protected ParticleSystem GetSub(ParticleSystem system)
        {
            var sube = system.subEmitters;
            return sube.GetSubEmitterSystem(0);
        }
    }
}
