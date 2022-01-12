using IgnitedBox.Utilities;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability.Cloning
{
    public class EnemySpawner : EnemyCloner
    {
        private static string GetRandomEnemy()
        {
            string path = "Regular/";
            int tiers = Resources.LoadAll("Enemies/" + path).Length;
            path += $"Tier{1 + Graphs.TierRange(tiers, 1.1f)}/";
            return path + Resources.LoadAll("Enemies/" + path)
                .RandomElement().name;
        }
        
        const float cooldown_time = 60;

        private float cooldown = 5;
        
        public EnemySpawner() : base(GetRandomEnemy(), true) { }

        public override void Initialize(BaseController self)
        {
            self.Events.Subscribe<BaseController, float>
                (BaseController.ControllerEvents.Update,
                    Spawner);
        }

        private void Spawner(BaseController self, float delta)
        {
            if(cooldown > 0)
            {
                cooldown -= delta;
                return;
            }

            Clone(self);

            cooldown = cooldown_time;
        }
    }
}
