using Scripts.OOP.Game_Modes;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns
{
    public interface IControllerElimenated
    {
        void Progress(AGameMode game, BaseController elimenated);
    }
}
