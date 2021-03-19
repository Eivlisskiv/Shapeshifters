using Scripts.OOP.Perks.Character.Triggers;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.Perks.Character
{
    public class Health_Regen : EmitterPerk, IControllerUpdate
    {
        float time;

        protected override int ToBuffCharge => 25;

        protected override string RessourcePath => "Particles/Healing";

        protected override string GetDescription()
            => $"Regenerate ({Intensity}) Heath points per 5 second.";

        public bool OnControllerUpdate(BaseController controller, float delta)
        {
            if (controller.stats.HPP >= 1) return false;

            if ((time += delta) >= 5f)
            {
                time -= 5f;
                controller.HitHealth(Intensity);
                Object.Destroy(SpawnPrefab(controller.transform.position), 1f);
            }
            return true;
        }
    }

    public class Thorns : EmitterPerk, ICollide
    {
        protected override string RessourcePath => "Particles/Sparks";

        protected override string GetDescription()
            => $"Colliding with enemies deals ({Intensity}) damage.";

        public bool OnCollide(BaseController controller, Collision2D collision)
        {
            BaseController other = collision.collider.GetComponent<BaseController>();
            if (other && !controller.IsTeammate(other))
            {
                Vector3 point = collision.contacts[0].point;
                other.TakeDamage(Intensity, controller, point);
                point.z = -5;
                Object.Destroy(SpawnPrefab(point), 1f); 
                return true;
            }
            return false;
        }
    }

    public class Shield : EmitterPerk, IProjectileTaken
    {
        protected override int ToBuffCharge => 1;

        protected override string RessourcePath => "Particles/Shields";

        protected override string GetDescription()
            => $"Has a ({Intensity}%) chance to block incoming projectiles.";

        public bool OnHit(BaseController self, ProjectileHandler projectile)
        {
            if (projectile && Randomf.Chance(Intensity))
            {
                var pos = projectile.transform.position;
                pos.z = -5;
                Object.Destroy(SpawnPrefab(pos), 1f);
                projectile.active = false;
                return true;
            }
            return false;
        }
    }
}
