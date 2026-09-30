using LeaseExtension.Common.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Common.Editor
{
    [CustomPropertyDrawer(typeof(IRef<>), true)]
    [MovedFrom("Aniki.Editor")]
    public class IRefDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty targetProp = property.FindPropertyRelative("_target");
            EditorGUI.ObjectField(position, targetProp, label);
        }
    }
}
