using IgnitedBox.Entities;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks.Character.Triggers;
using UnityEngine;

namespace Scripts.OOP.Perks.Character
{
    public class Thorns : EmitterPerk, ICollide
    {
        protected override string RessourcePath => "Particles/Sparks";

        protected override string GetDescription()
            => $"Colliding with enemies deals {Stat(Intensity)} damage.";

        public bool OnCollide(BaseController controller, Collision2D collision)
        {
            HealthEntity entity = collision.collider.GetComponent<HealthEntity>();
            if (!entity) return false;

            if(!(entity is BaseController other))
                other = collision.collider.GetComponent<BaseController>();

            if (other && controller.IsTeammate(other)) return false;

            Vector3 point = collision.contacts[0].point;
            point.z = -5;

            if (other) other.TakeDamage(Intensity, controller, point);
            else entity.ModifyHealth(-Intensity);

            UnityEngine.Object.Destroy(SpawnPrefab(point, null,
                GameModes.GetDebrisTransform(controller.Team)), 1f); 
            return true;
        }
    }
}
