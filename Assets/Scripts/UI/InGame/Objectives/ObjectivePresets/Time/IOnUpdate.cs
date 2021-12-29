using Scripts.OOP.Game_Modes.CustomLevels;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Time
{
    interface IOnUpdate
    {
        void Progress(CustomLevel game, float time);
    }
}
