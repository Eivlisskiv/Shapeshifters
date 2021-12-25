using IgnitedBox.Utilities;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets
{
    public abstract class ObjectivePreset : ObjectiveElement
    {
        private readonly static Dictionary<string, Type> Initializers
            = Reflection.GetImplements(typeof(ObjectivePreset))
            .ToDictionary((t) => t.Name.Replace('_', ' '));

        public static readonly string[] Preset_Ids = Initializers.Keys.ToArray();

        public static ObjectiveElement Create(ObjectiveData data)
            => ObjectiveHandler.Instance.CreateObjective(data.id, data.color, data,
                objType: Initializers.TryGetValue(data.id, out Type t) ? t : null);

        protected CustomLevel Game { get; private set; }
        protected ObjectiveHandler Handler => ObjectiveHandler.Instance;

        protected ObjectivePreset(GameObject element, ObjectiveData data = null) : base(element) 
        {
            if (data != null) Initialize(data);
        }

        protected virtual void Initialize(ObjectiveData data)
        {
            if(data.track != null) Track = (Vector2)data.track;
            Game = (CustomLevel)GameModes.GameMode;
        }

        protected override void OnRemoved(ObjectiveHandler handler)
        {
            base.OnRemoved(handler);
            Game.ObjectiveEvents.CleanInstace(this);
        }

        protected T LoadParam<T>(ObjectiveData data, int index, T dftl = default)
            => data.parameters != null && data.parameters.Length > index &&
                data.parameters[index] is T value ? value : dftl;
    }
}
