using Scripts.Items.InGame.Props.Targets;
using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Props.Specifics
{
    public class Spawn_Prop : ObjectivePreset, IOnPropActivation
    {
        ILevelProp prop;

        public Spawn_Prop(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            string prop_path = LoadParam<string>(data, 1);
            GameObject obj = Resources.Load<GameObject>(prop_path);
            if (obj) 
            {
                obj = Object.Instantiate(obj);
                prop = obj.GetComponent<ILevelProp>();

                int l = data.parameters.Length - 2;
                if (data.parameters != null && l > 0) 
                {
                    object[] prop_params = new object[l];
                    for (int i = 0; i < l; i++) prop_params[i] = data.parameters[i + 2];

                    prop.LoadParameters(prop_params); 
                }
                Game.InsertInMap(obj);

                Track = obj.transform;
            }

            Game.ObjectiveEvents.Subscribe<ILevelProp, GameObject>
                (typeof(IOnPropActivation), OnPropActivation);


            return base.Initialize(data);
        }

        public void OnPropActivation(ILevelProp prop, GameObject obj)
        {
            if (this.prop != prop) return;

            Completed();
        }

        protected override void OnReady()
        {
            base.OnReady();

            if (prop == null || prop.Consumed)
            {
                Completed();
                return;
            }
        }
    }
}
