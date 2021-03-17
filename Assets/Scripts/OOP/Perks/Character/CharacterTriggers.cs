using UnityEngine;

namespace Scripts.OOP.Perks.Character.Triggers
{
    public interface IControllerUpdate
    {
        bool OnControllerUpdate(BaseController controller, float delta);
    }

    public interface ICollide
    {
        bool OnCollide(BaseController controller, Collision2D collision);
    }

    public interface IProjectileTaken
    {
        bool OnHit(BaseController self, ProjectileHandler projectile);
    }
}
