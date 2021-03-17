using UnityEngine;
using UnityEngine.U2D;

namespace Scripts.OOP.ShapeController
{
    public class SplineTangent
    {
        const float min = 0.02f;
        const float velocityClamp = 0.5f;

        public enum Direction {  Left, Right };

        Direction direction;
        Vector2 origin;
        public Vector2 Origin
        {
            get => origin;
        }

        Vector2 velocity;

        public SplineTangent(Direction d, Vector2 point, Vector2 next)
        {
            direction = d;
            origin = (next - point) / 2;
            velocity = Vector2.zero;
        }

        public void AddVelocity(Vector2 vector)
            => velocity += vector;

        public void SetPosition(Spline spline, int index, Vector2 vect)
        {
            if (direction == Direction.Left)
                spline.SetLeftTangent(index, vect);
            else spline.SetRightTangent(index, vect);
        }

        public Vector2 GetPosition(Spline spline, int index)
             => direction == Direction.Left ?
                spline.GetLeftTangent(index) :
                spline.GetRightTangent(index);

        internal Vector2 UpdateVelocity(int index, Spline spline, float elasticity)
        {
            Vector2 current = GetPosition(spline, index);
            float m = velocity.magnitude;

            if (m != 0)
            {
                velocity -= (velocity * 0.99f) * Time.deltaTime;
                if (m < min) velocity = Vector2.zero;
            }

            Vector2 distance = origin - current;

            //If there is no velocity and distance is just really low
            if (velocity.magnitude == 0 && distance.magnitude < min)
            {
                SetPosition(spline, index, origin);
                return origin;
            }

            velocity += (distance * 10 * elasticity) * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, velocityClamp);

            Vector2 npos = current + (velocity * Time.deltaTime);
            SetPosition(spline, index, npos);
            return npos;
        }
    }
}
