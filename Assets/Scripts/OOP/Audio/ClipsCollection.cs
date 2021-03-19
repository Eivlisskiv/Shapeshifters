namespace Scripts.OOP.Audio
{
    public abstract class ClipsCollection
    {
        public string name;
        public AudioEntity[] clips;

        public abstract void Initialize();
    }
}
