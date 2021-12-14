using IgnitedBox.Utilities;
using Scripts.OOP.Game_Modes.Story;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets
{
    public static class ObjectivePreset
    {
        public static readonly string[] Preset_Ids = Initializers.Keys.ToArray();

        private readonly static Dictionary<string, Type> Initializers
            = Reflection.GetImplements(typeof(ObjectiveElement))
            .ToDictionary((t) => t.Name.Replace('_', ' '));

        public static ObjectiveElement Create(ObjectiveData data)
            => ObjectiveHandler.Instance.CreateObjective(data.id, data.color, data,
                objType: Initializers.TryGetValue(data.id, out Type t) ? t : null);
    }
}
