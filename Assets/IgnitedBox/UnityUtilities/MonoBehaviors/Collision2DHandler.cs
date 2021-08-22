using System;
using UnityEngine;

namespace IgnitedBox.UnityUtilities
{
    public class Collision2DHandler : MonoBehaviour
    {
        bool isOnStay = false;
        float onStayCooldown;
        float stayTime;

        public Collider2D Collider { get; private set; }

        Action<Collision2D> OnCollide { get; set; }
        Action<Collider2D> OnTrigger { get; set; }

        private void Start()
        {
            if (!Collider) Collider = GetComponent<Collider2D>();
        }

        public void Set<T>(float stayCooldown, Action<Collider2D> onTrigger) where T : Collider2D
        {
            Collider = GetComponent<Collider2D>();
            if (!Collider) Collider = gameObject.AddComponent<T>();
            Collider.isTrigger = true;
            OnTrigger = onTrigger;
            isOnStay = true;
            onStayCooldown = stayCooldown;
        }

        public void Set<T>(Action<Collider2D> onTrigger) where T : Collider2D
        {
            Collider = GetComponent<Collider2D>();
            if (!Collider) Collider = gameObject.AddComponent<T>();
            Collider.isTrigger = true;
            OnTrigger = onTrigger;
            isOnStay = false;
        }

        public void Set<T>(Action<Collision2D> onCollide) where T : Collider2D
        {
            Collider = GetComponent<Collider2D>();
            if (!Collider) Collider = gameObject.AddComponent<T>();
            Collider.isTrigger = false;
            OnCollide = onCollide;
            isOnStay = false;
        }

        public void Set<T>(float stayCooldown, Action<Collision2D> onCollide) where T : Collider2D
        {
            Collider = GetComponent<Collider2D>();
            if (!Collider) Collider = gameObject.AddComponent<T>();
            Collider.isTrigger = false;
            OnCollide = onCollide;
            isOnStay = true;
            onStayCooldown = stayCooldown;
        }

        private bool StayReady()
        {
            if (onStayCooldown <= 0) return true;

            stayTime -= Time.deltaTime;

            if (stayTime > 0) return false;

            stayTime = onStayCooldown;
            return true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isOnStay) return;

            OnTrigger?.Invoke(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!isOnStay || !StayReady()) return;
            OnTrigger?.Invoke(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isOnStay) return;

            OnCollide?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!isOnStay || !StayReady()) return;

            OnCollide?.Invoke(collision);
        }
    }
}
