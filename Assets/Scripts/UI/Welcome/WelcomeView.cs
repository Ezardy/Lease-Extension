using JetBrains.Annotations;
using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Welcome
{
    [UsedImplicitly]
    internal class WelcomeView : View, IWelcomeView
    {
        public WelcomeView(PanelRenderer panelRenderer) : base(panelRenderer, "welcome")
        {}
    }
}