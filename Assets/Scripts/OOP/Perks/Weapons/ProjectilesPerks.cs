using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using Scripts.OOP.Stats;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.Perks.Weapons
{
    class Mine_Drop : EmitterPerk, IWeaponFire
    {
        protected override int ToBuffCharge => 1;
        protected override string RessourcePath => "Projectiles/Landmine";

        protected override string GetDescription()
            => $"Has a ({Intensity}%) chance to drop a mine when shooting.";

        public bool OnFire(float angle, Weapon weapon, WeaponStats _)
        {
            if (Randomf.Chance(Intensity * weapon.cooldown))
            {
                GameObject mine = SpawnPrefab(weapon.transform.position, 
                    new Vector3(0, 0, angle), weapon.transform.parent);
                Landmine landmine = mine.GetComponent<Landmine>();
                landmine.BodyCollider.isTrigger = true;
                var scale = landmine.transform.localScale;
                landmine.transform.localScale = Vector3.zero;
                landmine.transform.Tween<Transform, Vector3, ScaleTween>
                    (scale, 0.5f, 0.2f, callback: () => 
                    {
                        landmine.Activate(Intensity * 2, Intensity, weapon.GetComponent<BaseController>());
                        landmine.BodyCollider.isTrigger = false;
                    });
                return true;
            }
            return false;
        }
    }
}
