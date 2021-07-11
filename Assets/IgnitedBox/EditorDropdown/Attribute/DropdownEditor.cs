#if UNITY_EDITOR
using IgnitedBox.EditorDropdown.Utilities;
using UnityEditor;
using UnityEngine;

namespace IgnitedBox.EditorDropdown.Attribute
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownEditor : PropertyDrawer
    {
        private DropdownAttribute _attribute;

        public DropdownEditor() : base()
        {
            _attribute = (DropdownAttribute)attribute;
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            if(_attribute == null) _attribute = (DropdownAttribute)attribute;
            _attribute.Load(fieldInfo, prop);

            if (!_attribute.Valid) return;

            DrawDropdownOption(prop, position);
        }

        private void DrawDropdownOption(SerializedProperty prop, Rect position)
        {
            if (_attribute.Index == null) return;

            int index = _attribute.Index.Length == 1 ? 0 : prop.GetIndex();

            if (_attribute.Index.Length <= index)
            {
                _attribute.SetIndex();
                if (_attribute.Index.Length <= index) return;
            }

            int selected = _attribute.Index[index];

            if (selected < 0 || selected > _attribute.Count)
            {
                _attribute.SetValue(0, index);
                selected = _attribute.Index[index];
            }

            int nindex = EditorGUI.Popup(position, prop.displayName,
                selected, _attribute.Options);

            if (_attribute.SetValue(nindex, index))
                EditorUtility.SetDirty(prop.serializedObject.targetObject);
        }
    }
}
#endif
