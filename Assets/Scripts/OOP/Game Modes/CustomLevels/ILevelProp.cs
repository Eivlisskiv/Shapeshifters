namespace Scripts.OOP.Game_Modes.CustomLevels
{
    public interface ILevelProp
    {
        bool Enabled { get; set; }
        bool Consumed { get; }
        void LoadParameters(object[] param);
    }
}
