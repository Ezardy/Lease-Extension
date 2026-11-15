using System;
using System.Globalization;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aniki.Inspector.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public sealed class ReadOnlyDrawer : PropertyDrawer
    {
        private const float COPY_BUTTON_WIDTH = 45f;
        private const float BUTTON_SPACING = 4f;
        private const string STYLE_SHEET_PATH = "Assets/_Project/Packages/Aniki.Inspector/Editor/ReadOnlyDrawer.uss";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new();
            PropertyField field = new(property);
            Button copyButton = new(() => CopyValue(property)) { text = "Copy" };
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(STYLE_SHEET_PATH);

            container.styleSheets.Add(styleSheet);
            container.AddToClassList("read-only");
            field.AddToClassList("read-only__field");
            copyButton.AddToClassList("read-only__copy");
            field.SetEnabled(false);
            copyButton.SetEnabled(!property.hasMultipleDifferentValues);
            container.TrackPropertyValue(property, changed => copyButton.SetEnabled(!changed.hasMultipleDifferentValues));
            container.Add(field);
            container.Add(copyButton);

            return container;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect fieldPosition = position;
            fieldPosition.width -= COPY_BUTTON_WIDTH + BUTTON_SPACING;
            Rect buttonPosition = new(fieldPosition.xMax + BUTTON_SPACING, position.y, COPY_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.PropertyField(fieldPosition, property, label, true);
            }

            using (new EditorGUI.DisabledScope(property.hasMultipleDifferentValues))
            {
                if (GUI.Button(buttonPosition, "Copy"))
                {
                    CopyValue(property);
                }
            }
        }

        private static void CopyValue(SerializedProperty property)
        {
            property.serializedObject.UpdateIfRequiredOrScript();
            EditorGUIUtility.systemCopyBuffer = GetValueText(property);
        }

        private static string GetValueText(SerializedProperty property)
        {
            string text;

            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                StringBuilder builder = new("[");

                for (int index = 0; index < property.arraySize; index++)
                {
                    if (index > 0)
                    {
                        builder.Append(", ");
                    }

                    builder.Append(GetValueText(property.GetArrayElementAtIndex(index)));
                }

                text = builder.Append(']').ToString();
            }
            else if (property.propertyType == SerializedPropertyType.Generic)
            {
                StringBuilder builder = new("{");
                using SerializedProperty child = property.Copy();
                using SerializedProperty end = property.GetEndProperty();
                bool hasChild = child.Next(true);

                while (hasChild && !SerializedProperty.EqualContents(child, end))
                {
                    if (builder.Length > 1)
                    {
                        builder.Append(", ");
                    }

                    builder.Append(child.name).Append(": ").Append(GetValueText(child));
                    hasChild = child.Next(false);
                }

                text = builder.Append('}').ToString();
            }
            else
            {
                object value = property.boxedValue;
                text = value switch
                {
                    null => "null",
                    string stringValue => stringValue,
                    UnityEngine.Object objectValue => objectValue != null ? objectValue.name : "null",
                    IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
                    _ => Convert.ToString(value, CultureInfo.InvariantCulture)
                };
            }

            return text;
        }
    }
}
