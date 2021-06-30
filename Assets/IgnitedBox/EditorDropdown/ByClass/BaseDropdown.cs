using System;

namespace IgnitedBox.EditorDropdown.ByClass
{
    [Serializable]
    public abstract class BaseDropdown
    {
        public abstract bool Valid { get; }

        public abstract string[] Options { get; protected set; }

        public abstract int Index { get; set; }

        public abstract string Selected { get; protected set; }

        public abstract void UpdateValue();
    }
}
