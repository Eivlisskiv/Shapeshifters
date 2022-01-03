using Scripts.OOP.Game_Modes.CustomLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Props
{
    public class Props_Activation : ObjectivePreset, IOnPropActivation
    {
        protected class PropData
        {
            public readonly GameObject obj;
            public readonly ILevelProp prop;

            public PropData(GameObject propObject, ILevelProp prop)
            {
                obj = propObject;
                this.prop = prop;
            }
        }

        protected Dictionary<string, PropData> targets;

        private int total;
        private Text counter;

        public Props_Activation(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            targets = new Dictionary<string, PropData>();

            Text title = base.Initialize(data);

            if (data.parameters == null) return title;

            for (int i = 1; i < data.parameters.Length; i++)
            {
                string name = LoadParam<string>(data, i);
                if (!Game.TryGetProp(name, out GameObject propObject) || !propObject)
                    continue;

                ILevelProp prop = propObject.GetComponent<ILevelProp>();

                if (prop == null || prop.Consumed) continue;

                if (!Track.HasValue) Track = propObject.transform;

                targets.Add(name, new PropData(propObject, prop));

            }

            total = targets.Count;

            counter = Get<Text>("Counter", t =>
            {
                t.text = "0/" + total;
            });

            Game.ObjectiveEvents.Subscribe<ILevelProp, GameObject>
                (typeof(IOnPropActivation), OnPropActivation);

            /*
              
            if (GameModes.GameMode is CustomLevel level)
                level.ObjectiveEvents.Invoke(typeof(IOnPropActivation), this, gameObject);

             */

            return title;
        }

        public void OnPropActivation(ILevelProp prop, GameObject obj)
        {
            string id = obj.name;
            if (targets.ContainsKey(id)) targets.Remove(id);

            counter.text = (total - targets.Count) + "/" + total;

            if (targets == null || targets.Count == 0)
            {
                Completed();
                return;
            }

            Bounce();

            if (!Track.HasValue || Track.Value.Equals(obj.transform))
            {
                Track = targets.First().Value.obj.transform;
            }

        }

        protected override void OnReady()
        {
            base.OnReady();
            if(targets == null || targets.Count == 0)
            {
                Completed();
            }
        }
    }
}
