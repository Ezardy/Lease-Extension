using UnityEngine.UIElements;

namespace LeaseExtension.UI.Contract
{
    internal interface IOverView : IView
    {
        public Button AcceptButton { get; }
    }
}
