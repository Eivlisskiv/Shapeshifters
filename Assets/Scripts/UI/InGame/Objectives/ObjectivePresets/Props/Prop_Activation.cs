using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Props
{
    public class Prop_Activation : PropTarget, IOnPropActivation
    {
        public Prop_Activation(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            //Subscribe to Game.Prop_Activated

            Game.ObjectiveEvents.Subscribe<ILevelProp, GameObject>
                (typeof(IOnPropActivation), OnPropActivation);

            return base.Initialize(data);

            //in prop, during "activation", call the event
            /*
             if (GameModes.GameMode is CustomLevel level)
                level.ObjectiveEvents.Invoke(typeof(IOnPropActivation), this, gameObject);
             */
        }

        public void OnPropActivation(ILevelProp prop, GameObject obj)
        {
            if (obj.name != Target) return;

            Completed();
        }

        protected override void OnReady()
        {
            base.OnReady();

            //If prop could not be found, auto complete the objective not to be stuck
            if(!propObject || prop.Consumed)
            {
                Completed();
                return;
            }
        }
    }
}
