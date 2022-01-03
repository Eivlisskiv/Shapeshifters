using Scripts.Items.InGame.Props.Targets;
using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Props.Specifics
{
    public class Spawn_Target : ObjectivePreset, IOnPropActivation
    {
        ILevelProp prop;

        public Spawn_Target(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            GameObject obj = Resources.Load<GameObject>("Props/Targets/TargetPractice");
            if (obj) 
            {
                obj = Object.Instantiate(obj);
                TargetPractice target = obj.GetComponent<TargetPractice>();
                target.LoadParameters(new object[] { LoadParam(data, 1, 10), LoadParam(data, 2, 10) });
                prop = target;

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
