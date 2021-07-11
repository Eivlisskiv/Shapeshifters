using IgnitedBox.EditorDropdown.Attribute;
using System;
using UnityEngine;

namespace IgnitedBox.EditorDropdown
{
    public class Examples : MonoBehaviour
    {
        public static string[] staticOptions =
        {
            "Static 1", "Static 2", "Static 3"
        };

        public static ChildExample staticChild = new ChildExample("Child A", "Child B", "Child C", "Child D");

        public GameObject[] internalOptions;

        [Dropdown("internalOptions")]
        public GameObject[] internalExample;

        [Dropdown(typeof(Examples), "staticOptions")]
        public string[] staticExample;
        

        public ChildExample child;

        [Dropdown("child.internalOptions")]
        public string[] childInternalExample;

        [Dropdown(typeof(Examples), "staticChild.internalOptions")]
        public string[] childStaticExample;
        
    }

    [Serializable]
    public class ChildExample
    {
        public string[] internalOptions;

        [Dropdown("internalOptions")]
        public string[] internalExample;

        public ChildExample() { }
        public ChildExample(params string[] options) 
        {
            internalOptions = options;
        }
    }
}
