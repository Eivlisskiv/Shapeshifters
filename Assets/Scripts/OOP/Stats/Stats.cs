namespace Scripts.OOP.Character.Stats
{
    public class Stats
    {
        public float health;
        private int maxHealth;

        public int MaxHealth { get => maxHealth; }

        public float HPP => health / maxHealth;

        public Stats(int health)
        {
            maxHealth = health;
            this.health = health;
        }
    }
}
