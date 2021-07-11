using System;
using UnityEngine;

namespace Scripts.Orbiters
{
    public abstract class OrbiterArchetype
    {
        enum OrbiterState { Cooldown, Charging, Firing, Idle }

        protected virtual float CooldownTime => 5;

        protected virtual float ChargeTime => 3;

        protected virtual float FireDuration => 2;

        public float WaitTime { get; protected set; }

        OrbiterState state;

        public virtual void Start<TOrbiter>(TOrbiter orbiter)
            where TOrbiter : Orbiter
        {
            SetState(OrbiterState.Cooldown);
        }

        private void SetState(OrbiterState state)
        {
            this.state = state;
            WaitTime = state == OrbiterState.Idle
                ? 0 : GetTime(state);
            Debug.Log($"State Changed to {state}");
            OnState(state);
        }

        private void OnState(OrbiterState state)
        {
            switch (state)
            {
                case OrbiterState.Cooldown: OnCooldown(); return;
                case OrbiterState.Charging: OnCharge(ChargeTime); return;
                case OrbiterState.Firing: OnFire(FireDuration); return;
                case OrbiterState.Idle: OnIdle(); return;
            }
        }

        protected abstract void OnCharge(float time);

        protected abstract void WhileCharge(Orbiter orbiter, float progress);

        protected abstract void OnFire(float time);

        protected abstract void WhileFire(Orbiter orbiter, float progress);

        protected abstract void OnCooldown();

        protected abstract void OnIdle();

        public abstract void OnRemove();

        public abstract void SetColor(Color color);

        public virtual BaseController FindTarget(Orbiter orbiter, BaseController currentTarget)
        {
            if (currentTarget) return currentTarget;

            if (orbiter.Owner is EnemyController enemy)
                return enemy.target;

            //Target Lost

            if(state == OrbiterState.Charging)
            {
                state = OrbiterState.Idle;
                OnIdle();
            }

            return null;
        }

        public void Update(Orbiter orbiter)
        {
            if (state != OrbiterState.Idle && !Continue(orbiter)) return;

            if (!orbiter.Target) return;

            //Proceed with firing process

            if (state == OrbiterState.Idle ||
                state == OrbiterState.Cooldown)
                SetState(OrbiterState.Charging);

            else if (state == OrbiterState.Charging)
                SetState(OrbiterState.Firing);

            else if (state == OrbiterState.Firing)
                SetState(OrbiterState.Cooldown);
        }

        private bool Continue(Orbiter orbiter)
        {
            //Has finished
            if (AwaitTime(out float norm)) return true;

            if (state == OrbiterState.Charging)
                WhileCharge(orbiter, norm);

            if (state == OrbiterState.Firing)
                WhileFire(orbiter, norm);

            return false;
        }

        private bool AwaitTime(out float norm)
        {
            if (WaitTime > 0)
            {
                WaitTime -= Time.deltaTime;

                norm = 1 - (WaitTime / GetTime(state));

                return false;
            }

            norm = 1;

            return true;
        }

        private float GetTime(OrbiterState state)
        {
            switch (state)
            {
                case OrbiterState.Cooldown: return CooldownTime;
                case OrbiterState.Charging: return ChargeTime;
                case OrbiterState.Firing: return FireDuration;
                default: return 1;
            };
        }
    }
}
