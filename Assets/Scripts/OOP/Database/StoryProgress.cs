namespace Scripts.OOP.Database
{
    public class StoryProgress : LevelProgress
    {
        public static StoryProgress Load(int chapter, int episode)
        {
            string id = $"IBS/{chapter}.{episode}";
            return Load<StoryProgress>(id, true);
        }
    }
}
