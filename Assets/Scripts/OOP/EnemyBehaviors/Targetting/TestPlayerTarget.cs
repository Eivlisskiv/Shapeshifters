namespace Scripts.OOP.EnemyBehaviors.Targetting
{
    public class TestPlayerTarget : ITargetBehavior
    {
        public BaseController Target(BaseController self)
            => TestPlayerSpawner.player;
    }
}
