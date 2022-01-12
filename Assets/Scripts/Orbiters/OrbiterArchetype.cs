using Assets.IgnitedBox.UnityUtilities;
using Scripts.OOP.EnemyBehaviors;
using UnityEngine;

namespace Scripts.Orbiters
{
    public abstract class OrbiterArchetype
    {
        protected enum OrbiterState { Cooldown, Charging, Firing, Idle }

        protected virtual float CooldownTime => 5;

        protected virtual float ChargeTime => 3;

        protected virtual float FireDuration => 2;

        public float WaitTime { get; protected set; }

        protected OrbiterState State { get; private set; }

        protected Orbiter SelfOrbiter { get; private set; }

        private readonly ITargetBehavior targetting;

        protected OrbiterArchetype(ITargetBehavior targetting)
        {
            this.targetting = targetting;
        }

        public virtual void Start<TOrbiter>(TOrbiter orbiter)
            where TOrbiter : Orbiter
        {
            SelfOrbiter = orbiter;
            SetState(OrbiterState.Cooldown);
        }

        private void SetState(OrbiterState state)
        {
            var oldState = State;
            State = state;
            WaitTime = state == OrbiterState.Idle
                ? 0 : GetTime(state);
            OnState(state);
            OnStateChange(oldState);
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

        protected virtual void OnStateChange(OrbiterState oldState) { }

        protected abstract void OnCharge(float time);

        protected abstract void WhileCharge(float progress);

        protected abstract void OnFire(float time);

        protected abstract void WhileFire(float progress);

        protected abstract void OnCooldown();

        protected abstract void OnIdle();

        public abstract void OnRemove();

        public abstract void SetColor(Color color);

        public virtual BaseController FindTarget(BaseController currentTarget)
        {
            if (currentTarget && !TargetLost(currentTarget))
                return currentTarget;

            //Target Lost

            //Search for a new target
            currentTarget = targetting?.Target(SelfOrbiter.Owner);
            //switch to new target
            if (currentTarget != null) return currentTarget;

            //No other targets found, stop
            if (State == OrbiterState.Charging)
            {
                State = OrbiterState.Idle;
                OnIdle();
            }

            return null;
        }

        protected bool TargetLost(BaseController currentTarget)
            => !Raycast.CanSee(SelfOrbiter, currentTarget, 8);

        public void Update(Orbiter orbiter)
        {
            if (State != OrbiterState.Idle && !Continue()) return;

            if (!orbiter.Target) return;

            //Proceed with firing process

            if (State == OrbiterState.Idle ||
                State == OrbiterState.Cooldown)
                SetState(OrbiterState.Charging);

            else if (State == OrbiterState.Charging)
                SetState(OrbiterState.Firing);

            else if (State == OrbiterState.Firing)
                SetState(OrbiterState.Cooldown);
        }

        private bool Continue()
        {
            //Has finished
            if (AwaitTime(out float norm)) return true;

            if (State == OrbiterState.Charging)
                WhileCharge(norm);

            if (State == OrbiterState.Firing)
                WhileFire(norm);

            return false;
        }

        private bool AwaitTime(out float norm)
        {
            if (WaitTime > 0)
            {
                WaitTime -= Time.deltaTime;

                norm = 1 - (WaitTime / GetTime(State));

                return false;
            }

            norm = 1;

            return true;
        }

        private float GetTime(OrbiterState state)
        {
#pragma warning disable // Convert switch statement to expression
            switch (state)
            {
                case OrbiterState.Cooldown: return CooldownTime;
                case OrbiterState.Charging: return ChargeTime;
                case OrbiterState.Firing: return FireDuration;
                default: return 1;
            };
#pragma warning restore
        }
    }
}
