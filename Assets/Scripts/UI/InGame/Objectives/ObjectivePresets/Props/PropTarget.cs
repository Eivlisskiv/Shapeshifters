using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Props
{
    public class PropTarget : ObjectivePreset
    {
        protected string Target { get; private set; }

        protected GameObject propObject;

        protected ILevelProp prop;

        public PropTarget(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            Target = LoadParam(data, 1, "_<Unknown Target>");

            return base.Initialize(data); ;
        }

        protected override void OnReady()
        {
            base.OnReady();
            if (Game.TryGetProp(Target, out propObject) && propObject)
            {
                prop = propObject.GetComponent<ILevelProp>();
                Track = propObject.transform;
            }
        }
    }
}
