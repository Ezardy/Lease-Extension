using Aniki.Common;
using UnityEditor;
using UnityEngine;

namespace Aniki.Editor {
	[CustomPropertyDrawer(typeof(IRef<>), true)]
	public class IRefDrawer : PropertyDrawer {
		public override void	OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			SerializedProperty	targetProp = property.FindPropertyRelative("target");
	
			EditorGUI.ObjectField(position, targetProp, label);
		}
	}
}
