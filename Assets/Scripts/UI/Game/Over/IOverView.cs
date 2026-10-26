using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Over
{
    internal interface IOverView : IView
    {
        public Button Accept { get; }
    }
}