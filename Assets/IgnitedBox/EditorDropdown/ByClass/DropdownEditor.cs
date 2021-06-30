#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IgnitedBox.EditorDropdown.ByClass
{
    [CustomPropertyDrawer(typeof(BaseDropdown))]
    public class DropdownEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var field = new Dropdown<int, int>(new int[] { 1 });

            if (!field.Valid) return;

            if (field.Index < 0 || field.Index > field.Options.Length)
                field.Index = 0;

            EditorGUI.BeginProperty(position, label, property);
            field.Index = EditorGUILayout.Popup(field.Index, field.Options);
            EditorGUI.EndProperty();
        }
    }
}
#endif
