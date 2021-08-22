public class BurstFire : BurstWeapon
{
    public override void DefaultPreset()
    {
        cooldown = 0.8f;
        force = 10;
        totalDamage = 15;
        life = 3;
        speed = 25;

        accuracy = 10;
        totalShots = 3;
    }
}
