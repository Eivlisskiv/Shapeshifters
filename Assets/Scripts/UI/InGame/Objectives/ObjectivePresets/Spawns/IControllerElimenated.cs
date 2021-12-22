using Scripts.OOP.Game_Modes.Story;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns
{
    public interface IControllerElimenated
    {
        void Progress(CustomLevel game, BaseController elimenated);
    }
}
