namespace Scripts.OOP.Game_Modes.CustomLevels
{
    public interface ILevelProp
    {
        bool Consumed { get; }
        void LoadParameters(object[] param);
    }
}
