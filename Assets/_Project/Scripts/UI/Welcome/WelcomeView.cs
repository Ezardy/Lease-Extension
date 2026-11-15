using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.UIElements;
using Zenject;

namespace LeaseExtension.UI.Welcome
{
    [UsedImplicitly]
    internal class WelcomeView : IWelcomeView, IInitializable
    {
        private readonly PanelRendererFactory _panelRendererFactory;
        private View _view;

        public VisualElement Root => _view?.Root;

        public WelcomeView(PanelRendererFactory panelRendererFactory)
        {
            _panelRendererFactory = panelRendererFactory;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        private async UniTaskVoid InitializeAsync()
        {
            _view = new(await _panelRendererFactory.CreateAsync(), "welcome");
        }
    }
}