using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Props
{
    public class Prop_Activation : PropTarget
    {
        public Prop_Activation(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            //Subscribe to Game.Prop_Activated

            Game.ObjectiveEvents.Subscribe<ILevelProp, string>
                (typeof(Prop_Activation), OnPropActivated);

            return base.Initialize(data);

            //in prop, during "activation", call the event
            /*
             if (GameModes.GameMode is CustomLevel level)
                level.ObjectiveEvents.Invoke<ILevelProp, string>
                    (typeof(Prop_Activation), this, gameObject.name);
             */
        }

        private void OnPropActivated(ILevelProp source, string id)
        {
            if (id != Target) return;

            Handler.Remove(this);
        }

        protected override void OnReady()
        {
            base.OnReady();

            //If prop could not be found, auto complete the objective not to be stuck
            if(!propObject || prop.Consumed)
            {
                Handler.Remove(this);
                return;
            }
        }
    }
}
