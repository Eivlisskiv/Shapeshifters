﻿using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Fire
{
    public class ChargeTarget : IFireBehavior
    {
        public bool Fire(EnemyController self, out float angle)
        {
            angle = 0;
            if (self.FireReady && self.Weapon)
            {
                if (!self.target || !ShootTarget(self, out angle))
                {
                    if (Randomf.Chance(50)) return false;
                    angle = Random.Range(0, 360);
                }
                return true;
            }
            return false;
        }

        private bool ShootTarget(EnemyController self, out float angle)
        {
            angle = 0;
            Vector2 vt = self.target.transform.position;
            Vector2 pos = self.transform.position;
            Vector2 direction = vt - pos;
            float distance = direction.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(pos + (direction.normalized
                * (self.body.radius + 0.1f)), direction, distance);
            if (hit && hit.transform == self.target.transform)
            {
                angle = Vectors2.TrueAngle(Vector2.right, pos - vt);
                return true;
            }

            return false;
        }
    }
}
