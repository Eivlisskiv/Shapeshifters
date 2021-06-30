using Scripts.OOP.Game_Modes;
using System.Collections.Generic;

namespace Scripts.OOP.EnemyBehaviors.Targetting
{
    public class SingleTarget : ITargetBehavior
    {
        public BaseController Target(EnemyController self)
        {
            if (self.target) return self.target;
            List<BaseController> targets = GameModes.GameMode?.GetEnemies(self.team);
            if (targets == null || targets.Count == 0) return null;
            return targets[0] == null ? null : targets[0];
        }
    }
}
