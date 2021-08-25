using Scripts.OOP.Game_Modes;
using System.Collections.Generic;

namespace Scripts.OOP.EnemyBehaviors.Targetting
{
    public class SingleTarget : ITargetBehavior
    {
        public virtual BaseController Target(BaseController self)
        {
            if (self is EnemyController enemy && enemy.target) return enemy.target;

            List<BaseController> targets = GameModes.GameMode?.GetEnemies(self.Team);
            if (targets == null || targets.Count == 0) return null;
            return targets[0] == null ? null : targets[0];
        }
    }
}
