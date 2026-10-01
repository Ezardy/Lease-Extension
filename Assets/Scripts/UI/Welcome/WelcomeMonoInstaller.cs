using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Zenject;

namespace LeaseExtension.UI.Welcome
{
    internal class WelcomeMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private PanelRenderer _panelRenderer;

        [SerializeField]
        private AssetReferenceSprite _horizontalWelcome;

        [SerializeField]
        private AssetReferenceSprite _verticalWelcome;

        public override void InstallBindings()
        {
            Container.BindInstance(_panelRenderer);
            Container.BindInterfacesTo<WelcomeView>().AsSingle();
            Container.BindInterfacesTo<WelcomeViewModel>().AsSingle().WithArguments(
                _horizontalWelcome,
                _verticalWelcome);
        }
    }
}
