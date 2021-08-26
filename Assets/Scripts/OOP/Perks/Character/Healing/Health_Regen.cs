using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks.Character.Triggers;

namespace Scripts.OOP.Perks.Character.Healing
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
                controller.ModifyHealth(Intensity);
                UnityEngine.Object.Destroy(SpawnPrefab(controller.transform.position,
                    null, GameModes.GetDebrisTransform(controller.Team)), 1f);
            }
            return true;
        }
    }
}
