using LeaseExtension.Common.Utilities;
using UnityEditor;
using UnityEngine;

namespace LeaseExtension.Common.Editor
{
    [CustomPropertyDrawer(typeof(IRef<>), true)]
    public class IRefDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty targetProp = property.FindPropertyRelative("_target");
            EditorGUI.ObjectField(position, targetProp, label);
        }
    }
}
