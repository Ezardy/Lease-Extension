using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aniki.UI.Elements {
	[UxmlElement]
	public partial class IconProgressBar : VisualElement {
		public const string	ussClassName = "icon-progress-bar";
		public const string	progressBarUssClassName = "icon-progress-bar__unity-progress-bar";
		public const string	iconUssClassName = "icon-progress-bar__icon";

		private readonly Image			image;
		private readonly ProgressBar	progressBar;


		private Texture2D	icon;
		private float		lowValue;
		private float		highValue;
		private float		value;
		private string		title;

		[UxmlAttribute]
		public Texture2D	Icon {
			get => icon;
			set {
				icon = value;
				image.image = value;
			}
		}

		[UxmlAttribute, CreateProperty]
		public float	LowValue {
			get => lowValue;
			set {
				lowValue = value;
				progressBar.lowValue = value;
				Title = title;
			}
		}

		[UxmlAttribute, CreateProperty]
		public float	HighValue {
			get => highValue;
			set {
				highValue = value;
				progressBar.highValue = value;
				Title = title;
			}
		}

		[UxmlAttribute, CreateProperty]
		public float	Value {
			get => value;
			set {
				this.value = value;
				progressBar.value = value;
				Title = title;
			}
		}

		[UxmlAttribute]
		public string	Title {
			get => title;
			set {
				if (string.IsNullOrEmpty(value))
					progressBar.title = $"{Value}/{HighValue}";
				else {
					progressBar.title = value;
					title = value;
				}
			}
		}

		public IconProgressBar() {
			AddToClassList(ussClassName);

			image = new();
			image.AddToClassList(iconUssClassName);
			Add(image);

			progressBar = new();
			progressBar.AddToClassList(progressBarUssClassName);
			Add(progressBar);

			RegisterCallback<AttachToPanelEvent>(e => Title = title);
		}
	}
}
