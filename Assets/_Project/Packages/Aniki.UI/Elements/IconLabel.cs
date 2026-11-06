using Aniki.UI.Elements.Attribute;
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
		public const string	measurementUssClassName = "icon-label__measurement";

		private readonly Label	labelElement;
		private readonly Image	iconElement;
		private readonly MeasurementLabel	measurementLabel;

		private string		label;
		private Background	icon;
		private float	gap = 4;

		[CreateProperty]
		public Background	Icon {
			get => icon;
			set {
				icon = value;

				if (icon.texture != null) {
					iconElement.image = icon.texture;
				} else if (icon.sprite != null) {
					iconElement.sprite = icon.sprite;
				} else if (icon.renderTexture != null) {
					iconElement.image = icon.renderTexture;
				} else if (icon.vectorImage != null) {
					iconElement.vectorImage = icon.vectorImage;
				} else {
					iconElement.image = null;
					iconElement.sprite = null;
					iconElement.vectorImage = null;
				}

				RefreshMeasurement();
			}
		}

		[UxmlAttribute, CreateProperty]
		public string	Label {
			get => label;
			set {
				label = value;
				labelElement.text = value;
				RefreshMeasurement();
			}
		}

		[UxmlAttribute, CreateProperty, Min(0)]
		public float	Gap {
			get => gap;
			set {
				gap = Mathf.Max(0, value);
				RefreshMeasurement();
			}
		}

		[UxmlAttribute("icon"), IconType]
		private UnityEngine.Object	IconReference {
			get => Icon.GetSelectedImage();
			set {
				if (value is Texture2D texture) {
					Icon = Background.FromTexture2D(texture);
				} else if (value is Sprite sprite) {
					Icon = Background.FromSprite(sprite);
				} else if (value is RenderTexture renderTexture) {
					Icon = Background.FromRenderTexture(renderTexture);
				} else if (value is VectorImage vectorImage) {
					Icon = Background.FromVectorImage(vectorImage);
				} else {
					Icon = default;
				}
			}
		}

		public IconLabel() {
			AddToClassList(ussClassName);

			iconElement = new();
			iconElement.AddToClassList(iconUssClassName);
			Add(iconElement);

			labelElement = new();
			labelElement.AddToClassList(containerUssClassName);
			labelElement.AddToClassList(labelUssClassName);
			Add(labelElement);

			measurementLabel = new() { pickingMode = PickingMode.Ignore };
			measurementLabel.AddToClassList(measurementUssClassName);
			Add(measurementLabel);

			measurementLabel.RegisterCallback<GeometryChangedEvent>(evt => UpdateLayout());
			RegisterCallback<GeometryChangedEvent>(evt => UpdateLayout());
			RegisterCallback<CustomStyleResolvedEvent>(evt => UpdateLayout());
			RegisterCallback<AttachToPanelEvent>(evt => schedule.Execute(UpdateLayout));
		}

		private void RefreshMeasurement() {
			measurementLabel.text = string.Empty;
			measurementLabel.text = string.IsNullOrEmpty(label) ? "M" : label;
			UpdateLayout();
		}

		private bool IsHorizontal() {
			FlexDirection	direction = resolvedStyle.flexDirection;

			return direction == FlexDirection.Row || direction == FlexDirection.RowReverse;
		}

		private float GetIconAspectRatio() {
			Vector2	size = Vector2.zero;

			if (iconElement.image != null) {
				size = new(iconElement.image.width, iconElement.image.height);
			} else if (iconElement.sprite != null) {
				size = iconElement.sprite.rect.size;
			} else if (iconElement.vectorImage != null) {
				size = new(iconElement.vectorImage.width, iconElement.vectorImage.height);
			}

			return size.y > 0 ? size.x / size.y : 0;
		}

		private Vector2 MeasureLabel(float fontSize) {
			Vector2	size = measurementLabel.MeasureTextSize(string.IsNullOrEmpty(label) ? "M" : label, 0, MeasureMode.Undefined, 0, MeasureMode.Undefined, fontSize);

			if (float.IsNaN(size.x) || float.IsNaN(size.y)) {
				size = Vector2.zero;
			}

			return size;
		}

		private Vector2 MeasureContent(float fontSize) {
			Vector2	textSize = MeasureLabel(fontSize);
			float	aspectRatio = GetIconAspectRatio();
			Vector2	iconSize = new(textSize.y * aspectRatio, aspectRatio > 0 ? textSize.y : 0);
			float	spacing = GetGap(fontSize);

			if (string.IsNullOrEmpty(label)) {
				textSize = Vector2.zero;
			}

			Vector2	size = IsHorizontal()
				? new(textSize.x + iconSize.x + spacing, Mathf.Max(textSize.y, iconSize.y))
				: new(Mathf.Max(textSize.x, iconSize.x), textSize.y + iconSize.y + spacing);

			return size;
		}

		private float GetGap(float fontSize) {
			return fontSize > 0 && !string.IsNullOrEmpty(label) && GetIconAspectRatio() > 0 ? gap : 0;
		}

		private float GetFittedFontSize(float width, float height) {
			float	fontSize = Mathf.Max(0, resolvedStyle.fontSize);
			Vector2	size = MeasureContent(fontSize);

			if (size.x > width || size.y > height) {
				float	minimum = 0;
				float	maximum = fontSize;

				for (int i = 0; i < 12; i++) {
					float	candidate = (minimum + maximum) * 0.5f;
					Vector2	candidateSize = MeasureContent(candidate);

					if (candidateSize.x <= width && candidateSize.y <= height) {
						minimum = candidate;
					} else {
						maximum = candidate;
					}
				}

				fontSize = minimum;
			}

			return fontSize;
		}

		private void UpdateLayout() {
			if (panel != null && contentRect.width > 0 && contentRect.height > 0) {
				float	fontSize = GetFittedFontSize(contentRect.width, contentRect.height);
				Vector2	textSize = MeasureLabel(fontSize);
				float	aspectRatio = GetIconAspectRatio();
				Vector2	iconSize = new(textSize.y * aspectRatio, aspectRatio > 0 ? textSize.y : 0);
				Vector2	contentSize = MeasureContent(fontSize);
				float	spacing = GetGap(fontSize);
				bool	horizontal = IsHorizontal();
				bool	reverse = resolvedStyle.flexDirection == FlexDirection.RowReverse || resolvedStyle.flexDirection == FlexDirection.ColumnReverse;
				float	remaining = horizontal ? contentRect.width - contentSize.x : contentRect.height - contentSize.y;
				float	offset = resolvedStyle.justifyContent == Justify.Center ? remaining * 0.5f : resolvedStyle.justifyContent == Justify.FlexEnd ? remaining : 0;
				Vector2	origin = contentRect.position;

				if (string.IsNullOrEmpty(label)) {
					textSize = Vector2.zero;
				}

				if (horizontal) {
					origin += new Vector2(reverse ? remaining - offset : offset, (contentRect.height - contentSize.y) * 0.5f);
					SetBounds(iconElement, new(origin + new Vector2(reverse ? textSize.x + spacing : 0, 0), iconSize));
					SetBounds(labelElement, new(origin + new Vector2(reverse ? 0 : iconSize.x + spacing, 0), textSize));
				} else {
					origin += new Vector2((contentRect.width - contentSize.x) * 0.5f, reverse ? remaining - offset : offset);
					SetBounds(iconElement, new(origin + new Vector2((contentSize.x - iconSize.x) * 0.5f, reverse ? textSize.y + spacing : 0), iconSize));
					SetBounds(labelElement, new(origin + new Vector2((contentSize.x - textSize.x) * 0.5f, reverse ? 0 : iconSize.y + spacing), textSize));
				}

				labelElement.style.fontSize = fontSize;
			}
		}

		private static void SetBounds(VisualElement element, Rect bounds) {
			element.style.left = bounds.x;
			element.style.top = bounds.y;
			element.style.width = bounds.width;
			element.style.height = bounds.height;
		}

		private sealed class MeasurementLabel : Label {
			protected override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode) {
				IconLabel	owner = (IconLabel)parent;
				float	width = !owner.IsHorizontal() && widthMode != MeasureMode.Undefined ? desiredWidth : float.PositiveInfinity;
				float	height = owner.IsHorizontal() && heightMode != MeasureMode.Undefined ? desiredHeight : float.PositiveInfinity;
				float	fontSize = owner.GetFittedFontSize(width, height);
				Vector2	size = owner.MeasureContent(fontSize);

				return size;
			}
		}
	}
}
