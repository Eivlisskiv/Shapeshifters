#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IgnitedBox.EditorDropdown.ByAttribute
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    class DropdownEditor : PropertyDrawer
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

            if (_attribute.Index < 0 || _attribute.Index > _attribute.Count)
            {
                _attribute.SetValue(0);
            }

            int nindex = EditorGUI.Popup(position, prop.name, _attribute.Index, _attribute.Options);

            if(_attribute.SetValue(nindex))
                EditorUtility.SetDirty(prop.serializedObject.targetObject);
        }
    }
}
#endif
