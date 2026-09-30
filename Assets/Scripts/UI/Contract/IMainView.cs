using UnityEngine.UIElements;

namespace LeaseExtension.UI.Contract
{
    internal interface IMainView : IView
    {
        public Button StoreButton { get; }
        public VisualElement EarnedGroup { get; }
    }
}
