using System;
using Aniki.UI.Elements.Attribute;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aniki.UI.Editor {
	[CustomPropertyDrawer(typeof(IconTypeAttribute))]
	public sealed class IconTypeDrawer : PropertyDrawer {
		private enum IconType {
			Texture,
			Sprite,
			VectorImage,
			RenderTexture
		}

		public override VisualElement CreatePropertyGUI(SerializedProperty property) {
			VisualElement	container = new();
			IconType	iconType = property.objectReferenceValue switch {
				Sprite => IconType.Sprite,
				VectorImage => IconType.VectorImage,
				RenderTexture => IconType.RenderTexture,
				_ => IconType.Texture
			};
			EnumField	typeField = new("Icon Type", iconType);
			ObjectField	imageField = new(property.displayName) {
				objectType = GetObjectType(iconType),
				allowSceneObjects = false
			};

			typeField.AddToClassList(EnumField.alignedFieldUssClassName);
			imageField.AddToClassList(ObjectField.alignedFieldUssClassName);
			imageField.BindProperty(property);
			typeField.RegisterValueChangedCallback(evt => {
				imageField.objectType = GetObjectType((IconType)evt.newValue);

				if (imageField.value != null && !imageField.objectType.IsInstanceOfType(imageField.value)) {
					imageField.value = null;
				}
			});

			container.Add(typeField);
			container.Add(imageField);

			return container;
		}

		private static Type GetObjectType(IconType iconType) {
			Type	objectType = iconType switch {
				IconType.Sprite => typeof(Sprite),
				IconType.VectorImage => typeof(VectorImage),
				IconType.RenderTexture => typeof(RenderTexture),
				_ => typeof(Texture2D)
			};

			return objectType;
		}
	}
}
