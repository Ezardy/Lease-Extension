using LeaseExtension.Common.Utilities;
using UnityEditor;
using UnityEngine;

namespace LeaseExtension.Common.Editor
{
    [CustomPropertyDrawer(typeof(Ref<>), true)]
    public class RefDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty targetProp = property.FindPropertyRelative("_target");
            EditorGUI.ObjectField(position, targetProp, label);
        }
    }
}
