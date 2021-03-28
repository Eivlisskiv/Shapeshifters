namespace Scripts.OOP.Game_Modes
{
    interface IControllerLevelUp
    {
        void ControllerLevelUp(BaseController controller); 
    }

    interface IElimination
    {
        void Elimenation(BaseController victim, BaseController killer);
    }

    interface IRogueMenu
    {
        void MenuClosed();
    }
}
