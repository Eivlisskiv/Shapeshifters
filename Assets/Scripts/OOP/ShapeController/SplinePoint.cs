using UnityEngine;
using UnityEngine.U2D;

namespace Scripts.OOP.ShapeController
{
    public class SplinePoint
    {
        const float min = 0.02f;
        const float velocityClamp = 0.5f;

        int index;

        Vector2 origin;
        public Vector2 Origin
        {
            get => origin;
        }

        Vector2 current;
        public Vector2 Position
        {
            get => current;
        }

        Vector2 velocity;

        public SplineTangent left;
        public SplineTangent right;

        public SplinePoint(int index, Vector2 current, Vector2 leftv, Vector2 rightv)
        {
            this.index = index;
            origin = current;
            this.current = origin;
            velocity = Vector2.zero;

            left = new SplineTangent(SplineTangent.Direction.Left, origin, leftv);
            right = new SplineTangent(SplineTangent.Direction.Right, origin, rightv);
        }

        public void AddVelocity(Vector2 vector) 
            => velocity += vector;

        public Vector2 UpdateVelocity(Spline spline, float elasticity)
        {
            current = spline.GetPosition(index);
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
                spline.SetPosition(index, origin);
                return origin;
            }
            
            velocity += (distance * 10 * elasticity) * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, velocityClamp);

            current += (velocity * Time.deltaTime);
            spline.SetPosition(index, current);
            return current;
        }
    }
}
