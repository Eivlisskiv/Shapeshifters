using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.Utilities;
using Scripts.OOP.Stats;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.Perks.Weapons
{
    class Mine_Drop : EmitterPerk, IWeaponFire
    {
        const int base_force = 20;

        protected override int ToBuffCharge => 1;
        protected override string RessourcePath => "Projectiles/Landmine";

        private float SpawnChance =>
            Graphs.LimitedGrowthExponent(Intensity, 50, 0.99f, 10);

        private float Force =>
            Graphs.LimitedGrowthExponent(Intensity, 25 + base_force, 0.99f, base_force);

        protected override string GetDescription()
            => $"Has a ({Mathf.Floor(SpawnChance)}%) chance to drop a mine when firing. " +
            $"Mine's explosion deals up to ({Intensity * 2}) damage.";

        public bool OnFire(float angle, Weapon weapon, WeaponStats _)
        {
            if (Randomf.Chance(SpawnChance * weapon.cooldown))
            {
                GameObject mine = SpawnPrefab(weapon.transform.position, 
                    new Vector3(0, 0, angle), weapon.transform.parent);
                Landmine landmine = mine.GetComponent<Landmine>();
                landmine.BodyCollider.isTrigger = true;
                var scale = landmine.transform.localScale;
                landmine.transform.localScale = Vector3.zero;
                BaseController controller = weapon.GetComponent<BaseController>();
                landmine.transform.Tween<Transform, Vector3, ScaleTween>
                    (scale, 0.5f, 0.2f, callback: () => 
                    {
                        landmine.Activate(Intensity * 2, Force, controller);
                        landmine.BodyCollider.isTrigger = false;
                    });
                return true;
            }
            return false;
        }
    }
}
