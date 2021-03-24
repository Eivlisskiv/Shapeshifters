using Scripts.OOP.GameModes;
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
                Object.Destroy(SpawnPrefab(controller.transform.position,
                    null, AGameMode.GetDebrisTransform(controller.team)), 1f);
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
                Object.Destroy(SpawnPrefab(point, null,
                    AGameMode.GetDebrisTransform(controller.team)), 1f); 
                return true;
            }
            return false;
        }
    }

    public class Shield : EmitterPerk, IControllerUpdate, IProjectileTaken
    {
        private float shield = 0;
        private float cooldown = 0;

        protected override int ToBuffCharge => 25;

        protected override string RessourcePath => "Particles/Shields";

        protected override string GetDescription()
            => $"Grants ({Intensity * 5}) Shields with a 5 seconds recharge rate.";

        public bool OnHit(BaseController self, ProjectileHandler projectile)
        {
            cooldown = 5;

            if (!projectile || shield <= 0) return false;

            Transform debrisParent = AGameMode.GetDebrisTransform(self.team);

            if(shield >= projectile.damage)
            {
                shield -= projectile.damage;
                projectile.active = false;
                Shielded(projectile, debrisParent);
                return false;
            }

            projectile.damage -= shield;
            shield = 0;
            Shielded(projectile, debrisParent);
            return false;
        }

        private void Shielded(ProjectileHandler projectile, Transform debrisParent)
        {
            var pos = projectile.transform.position;
            pos.z = -5;
            Object.Destroy(SpawnPrefab(pos, null, debrisParent), 1f);
        }

        public bool OnControllerUpdate(BaseController controller, float delta)
        {
            if (cooldown > 0)
            {
                cooldown -= delta;
                if(cooldown > 0) return false;
            }

            float max = Intensity * 5;
            if (shield >= max) return false;

            shield = System.Math.Min(max, shield + delta);
            return true;
        }
    }
}
