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
                landmine.Activate(Intensity * 2, Intensity * 5, weapon.GetComponent<BaseController>());
                return true;
            }
            return false;
        }
    }
}
