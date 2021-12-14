using IgnitedBox.UnityUtilities;
using IgnitedBox.UnityUtilities.Vectors;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Fire
{
    public class TargetAim : IFireBehavior
    {
#if UNITY_EDITOR
        const bool SHOW_LINE = false;

        private LineRenderer aim;
#endif

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

            Vector2 origin = pos + (direction.normalized
                * (self.Body.Radius + 0.1f));

            RaycastHit2D hit = Physics2D.Raycast(origin, direction);

#if UNITY_EDITOR
#pragma warning disable CS0162 // Unreachable code detected
            if (SHOW_LINE)
            {
                if (!aim) CreateLine(self);
                SetLine(origin, hit.point);
            }
#pragma warning restore CS0162 // Unreachable code detected
#endif

            if (hit && hit.transform == self.target.transform)
            {
                angle = Vectors2.TrueAngle(Vector2.right, pos - vt);
                if (distance <= self.Weapon.Range) angle += 180;
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void CreateLine(EnemyController self)
        {
            aim = Components.CreateGameObject<LineRenderer>("Aim", self.transform);
            aim.widthMultiplier = 0.5f;
        }

        private void SetLine(Vector2 origin, Vector2 to)
        {
            if (!aim) return;

            aim.SetPositions(new Vector3[] { origin, to });
        }
#endif
    }
}
