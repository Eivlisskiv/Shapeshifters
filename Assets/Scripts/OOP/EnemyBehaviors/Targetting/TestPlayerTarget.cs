namespace Scripts.OOP.EnemyBehaviors.Targetting
{
    public class TestPlayerTarget : ITargetBehavior
    {
        public BaseController Target(EnemyController self)
            => TestPlayerSpawner.player;
    }
}
