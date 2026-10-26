using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Main
{
    internal interface IMainView : IView
    {
        public Button Store { get; }
    }
}