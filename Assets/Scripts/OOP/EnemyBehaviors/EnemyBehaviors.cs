﻿namespace Scripts.OOP.EnemyBehaviors
{
    public interface ITargetBehavior
    {
        BaseController Target(BaseController self);
    }

    public interface IFireBehavior
    {
        bool Fire(EnemyController self, out float angle);
    }

    public interface IAbilitybehavior
    {
        void Ability(BaseController self);
    }
}