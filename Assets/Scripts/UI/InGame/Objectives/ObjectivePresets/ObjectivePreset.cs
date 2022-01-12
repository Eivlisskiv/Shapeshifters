using IgnitedBox.Utilities;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        private readonly int scoreReward;

        protected ObjectivePreset(GameObject element, ObjectiveData data = null) : base(element) 
        {
            if(GameModes.GameMode is CustomLevel level) Game = level;

            if (data != null)
            {
                if (data.track != null) Track = (Vector2)data.track;
                scoreReward = Math.Max(data.scoreReward, 0);
                Initialize(data);
            }
        }

        protected virtual Text Initialize(ObjectiveData data)
        {
            return Get<Text>("Title", t =>
            {
                t.text = LoadParam(data, 0, "_<Unknown Objective>");
                t.alignment = TextAnchor.MiddleCenter;
            });
        }

        protected override void OnRemoved(ObjectiveHandler handler)
        {
            base.OnRemoved(handler);
            Game?.ObjectiveEvents?.CleanInstace(this);
        }

        protected virtual void Completed()
        {
            if(Game != null) Game.Score += scoreReward;
            Handler.Remove(this);
        }

        protected T LoadParam<T>(ObjectiveData data, int index, T dftl = default)
            => data.parameters != null && data.parameters.Length > index &&
                data.parameters[index] is T value ? value : dftl;
    }
}
