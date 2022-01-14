namespace Scripts.OOP.EnemyBehaviors.Ability.Cloning
{
    public class Number_Four : EnemyCloner, IBossAbility
    {
        public Number_Four() : base("Boss/Number Four", true, 10) { }

        public override void Initialize(BaseController self)
        {
            self.Events.Subscribe<BaseController, BaseController>
                (BaseController.ControllerEvents.DamageTaken, DamageTaken);
        }

        public void DamageTaken(BaseController self, BaseController attacker)
        {
            if (spawns >= maxSpawns) return;
            float next = 1f - ((1f / maxSpawns) * (spawns + 1));
            if (self.stats.HPP > 0 && self.stats.HPP <= next)
            {
                EnemyController clone = Clone(self);
                clone.settings.baseHealth = (int)(-self.stats.HPP * self.MaxHealth);
                clone.settings.bonusHealth = 0;
                clone.settings.abilityBehavior = "NoAbility";
            }
        }
    }
}