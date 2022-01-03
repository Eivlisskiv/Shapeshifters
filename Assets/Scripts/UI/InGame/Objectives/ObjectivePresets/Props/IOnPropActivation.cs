using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Props
{
    public interface IOnPropActivation
    {
        void OnPropActivation(ILevelProp prop, GameObject obj);
    }
}
