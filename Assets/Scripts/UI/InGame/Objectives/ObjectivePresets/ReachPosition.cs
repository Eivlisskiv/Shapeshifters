using Scripts.OOP.Game_Modes.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets
{
    public class Reach_Position : ObjectiveElement
    {
        public Reach_Position(GameObject element) : base(element) { }

        public override void Initialize(ObjectiveData data)
        {
            base.Initialize(data);
        }
    }
}
