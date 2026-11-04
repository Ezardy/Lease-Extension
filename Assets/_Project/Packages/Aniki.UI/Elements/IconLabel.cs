using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aniki.UI.Elements {
	[UxmlElement]
	public partial class IconLabel : VisualElement {
		public const string	ussClassName = "icon-label";
		public const string	iconUssClassName = "icon-label__icon";
		public const string	containerUssClassName = "icon-label__container";
		public const string	labelUssClassName = "icon-label__label";

		private readonly Label	labelElement;
		private readonly Image	iconElement;

		private string		label;
		private Texture2D	icon;

		[UxmlAttribute]
		public Texture2D	Icon {
			get => icon;
			set {
				icon = value;
				iconElement.image = icon;
			}
		}

		[UxmlAttribute, CreateProperty]
		public string	Label {
			get => label;
			set {
				label = value;
				labelElement.text = value;
			}
		}

		public IconLabel() {
			VisualElement	container = new();

			AddToClassList(ussClassName);

			iconElement = new();
			iconElement.AddToClassList(iconUssClassName);
			Add(iconElement);

			container.AddToClassList(containerUssClassName);
			Add(container);

			labelElement = new();
			labelElement.AddToClassList(labelUssClassName);
			container.Add(labelElement);
		}
	}
}
