using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Control
{
    [UxmlElement]
    public partial class SelectableScrollView : ScrollView
    {
        private const string CONTROL_CLASS = "selectable-scroll-view";
        private const string CONTENT_CLASS = "selectable-scroll-view__content";
        private const string ITEM_CLASS = "selectable-scroll-view__item";
        private const string SELECTED_ITEM_CLASS = "selectable-scroll-view__item--selected";

        [UxmlAttribute("item-template")] public VisualTreeAsset ItemTemplate { get; set; }
        public VisualElement SelectedItem { get; private set; }
        public event Action<VisualElement> OnSelectionChanged;
        public event Action OnSelectionCleared;

        public SelectableScrollView() : base(ScrollViewMode.Horizontal)
        {
            AddToClassList(CONTROL_CLASS);
            contentContainer.AddToClassList(CONTENT_CLASS);
        }

        public void SetItems<T>(IEnumerable<T> items, Action<VisualElement, T> bindItem)
        {
            if (ItemTemplate == null)
            {
                throw new InvalidOperationException("Assign an item template before setting items.");
            }

            ClearItems();
            foreach (T item in items)
            {
                TemplateContainer element = ItemTemplate.Instantiate();
                element.AddToClassList(ITEM_CLASS);
                element.focusable = true;
                element.AddManipulator(new Clickable(() => SelectItem(element)));
                element.RegisterCallback<NavigationSubmitEvent>(_ => SelectItem(element));
                bindItem(element, item);
                Add(element);
            }
        }

        public void ClearItems()
        {
            ClearSelection();
            if (childCount > 0)
            {
                Clear();
            }
            scrollOffset = Vector2.zero;
        }

        public void SelectItem(VisualElement element)
        {
            SelectedItem?.RemoveFromClassList(SELECTED_ITEM_CLASS);
            SelectedItem = element;
            SelectedItem.AddToClassList(SELECTED_ITEM_CLASS);
            ScrollTo(element);

            OnSelectionChanged?.Invoke(element);
        }

        public void ClearSelection()
        {
            SelectedItem?.RemoveFromClassList(SELECTED_ITEM_CLASS);
            SelectedItem = null;
            OnSelectionCleared?.Invoke();
        }
    }
}
