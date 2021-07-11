namespace Scripts.OOP.EnemyBehaviors
{
    public interface ITargetBehavior
    {
        BaseController Target(EnemyController self);
    }

    public interface IFireBehavior
    {
        bool Fire(EnemyController self, out float angle);
    }

    public interface IAbilitybehavior
    {
        void Ability(EnemyController self);
    }
}
