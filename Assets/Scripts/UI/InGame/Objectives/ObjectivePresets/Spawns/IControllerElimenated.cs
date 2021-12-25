using Scripts.OOP.Game_Modes.CustomLevels;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns
{
    public interface IControllerElimenated
    {
        void Progress(CustomLevel game, BaseController elimenated);
    }
}
