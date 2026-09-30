using LeaseExtension.UI.Contract;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Welcome
{
    [MovedFrom("Aniki.UI")]
    internal class WelcomeView : AView, IWelcomeView
    {
        public WelcomeView(PanelRenderer panelRenderer) : base(panelRenderer, "welcome")
        {
        }
    }
}
