using Scripts.Orbiters.Eye;

namespace Scripts.OOP.EnemyBehaviors.Ability.OrbiterSpawners
{
    public class EyeSpawner : OrbiterSpawner<EyeOrbiter>
    {
        public EyeSpawner() : base() { }
    }

    public class Pyramid : OrbiterSpawner<EyeOrbiter>
    {
        public Pyramid() : base(1, 3) { }
    }
}
