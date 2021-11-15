using Assets.IgnitedBox.UnityUtilities;
using IgnitedBox.Entities;
using System;
using UnityEngine;

namespace Scripts.Explosion
{
    [Serializable]
    public class ExplosionHandler : MonoBehaviour
    {
        public ParticleSystem mainExplosion;

        protected CircleCollider2D Collider
            => _collider ? _collider : CreateForce();

        private CircleCollider2D _collider;

        private CircleCollider2D CreateForce()
        {
            _collider = gameObject.AddComponent<CircleCollider2D>();
            _collider.isTrigger = true;
            return _collider;
        }

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

        public float damage = 10;
        public int ignoreTeam = -1;

        enum State { Stopped, Starting, Playing }

        private State playing;
        private float time;
        private float Progress => time / Duration;

        private void Update()
        {
            if (playing == State.Stopped) return;
            if (playing == State.Starting)
            {
                playing = State.Playing;
                Collider.enabled = true;
                time += Time.deltaTime;
                return;
            }

            time += Time.deltaTime;

            Collider.radius = Range * Progress;

            if (time > Duration)
            {
                Stop();
            }
        }

        public virtual void FullStop()
        {
            if (!MainEffect.isPlaying) return;

            Stop();

            MainEffect.Stop(true,
                ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public virtual void Stop()
        {
            if (!MainEffect.isPlaying) return;

            playing = State.Stopped;
            Collider.radius = 0;
            Collider.enabled = false;
            time = 0;
        }

        public virtual void Play()
        {
            if (!MainEffect || MainEffect.isPlaying) return;
            playing = State.Starting;
            MainEffect.Play();
        }

        public void Restart()
        {
            Stop();
            Play();
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
            burst.count = Math.Min(10, (int)(value/10));
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

        private void OnTriggerEnter2D(Collider2D collision)
            => OnForceHit(collision);

        protected virtual void OnForceHit(Collider2D collision)
        {
            Rigidbody2D body = collision.attachedRigidbody;

            if (!body) return;

            Vector2 pos = transform.position;
            Vector2 target = body.transform.position;
            Vector2 dist = target - pos;

            //If false, nothing was hit... should not happen: where is target?
            if (!Raycast.TryRaycast2D(pos, dist, out RaycastHit2D ray)) return;

            //If what was hit is static: target is "in cover" from the explosion
            if (!ray.rigidbody || ray.rigidbody.bodyType == RigidbodyType2D.Static) return;

            float mult = 1;

            //If another entity is in the way: reduce the explosion's effect
            while (mult > 0 && ray && ray.rigidbody != body)
            {
                Vector2 obstacle = ray.rigidbody.transform.position;
                Vector2 diff = target - obstacle;
                mult -= diff.magnitude / dist.magnitude;
                Raycast.TryRaycast2D(obstacle, dist, out ray);
            }

            if (body)
            {
                Vector2 force = (body.transform.position - transform.position) * 0.5f;
                body.AddForce(force * (1 - Progress) * Intensity * mult, ForceMode2D.Impulse);
            }

            BaseController controller = collision.GetComponent<BaseController>();
            if (controller && controller.Team == ignoreTeam) return;

            HealthEntity he = controller ? controller : collision.GetComponent<HealthEntity>();
            if (he)
            {
                he.ModifyHealth(damage * (1 - Progress) * mult);
            }
        }
    }
}
