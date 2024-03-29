﻿using IgnitedBox.Utilities;
using Scripts.OOP.EnemyBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Orbiters.Eye
{
    public abstract class EyeOrbiterType : OrbiterArchetype
    {
        public static readonly Dictionary<string, Type> types = typeof(EyeOrbiterType)
            .GetImplements().ToDictionary(t => t.Name.Replace('_', ' '));

        protected Transform Pupil { get; set; }

        protected EyeOrbiterType(ITargetBehavior targetting) 
            : base(targetting) { }
    }
}
