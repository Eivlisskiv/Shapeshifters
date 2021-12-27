using Scripts.OOP.Game_Modes.CustomLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Other
{
    public class Prop_Activation : ObjectivePreset
    {
        private string propTarget;

        public Prop_Activation(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override void Initialize(ObjectiveData data)
        {
            base.Initialize(data);

            propTarget = LoadParam(data, 1, "_<Unknown Target>");

            Get<Text>("Title", t => t.text = LoadParam(data, 0, $"Activate Target Item [{propTarget}]"));

            //Subscribe to Game.Prop_Activated

            Game.ObjectiveEvents.Subscribe<ILevelProp, string>
                (typeof(Prop_Activation), OnPropActivated);

            //in prop, during "activation", call the event
            /*
             if (GameModes.GameMode is CustomLevel level)
                level.ObjectiveEvents.Invoke<ILevelProp, string>
                    (typeof(Prop_Activation), this, gameObject.name);
             */
        }

        private void OnPropActivated(ILevelProp source, string id)
        {
            if (id != propTarget) return;

            Handler.Remove(this);
        }

        protected override void OnReady()
        {
            base.OnReady();

            //If prop could not be found, auto complete the objective not to be stuck
            if(!Game.TryGetProp(propTarget, out GameObject obj) || //Or if the prop was already used
                (obj.TryGetComponent(out ILevelProp prop) && prop.Consumed))
            {
                Handler.Remove(this);
                return;
            }
        }
    }
}
