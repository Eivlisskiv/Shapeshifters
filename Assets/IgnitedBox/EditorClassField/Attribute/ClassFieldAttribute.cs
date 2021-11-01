using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IgnitedBox.EditorClassField.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ClassFieldAttribute : PropertyAttribute
    {

    }
}
