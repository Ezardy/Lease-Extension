using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Zenject;

namespace LeaseExtension.UI.Welcome
{
    internal class WelcomeUIMonoInstaller : MonoInstaller
    {
        [SerializeField] private AssetReferenceGameObject _panelRendererPrefab;
        [SerializeField] private AssetReferenceSprite _horizontalWelcome;
        [SerializeField] private AssetReferenceSprite _verticalWelcome;

        public override void InstallBindings()
        {
            Container.BindFactory<PanelRenderer, PanelRendererFactory>()
                .FromComponentInAssetReference(_panelRendererPrefab);
            Container.BindInterfacesTo<WelcomeView>().AsSingle();
            Container.BindInterfacesTo<WelcomeViewModel>().AsSingle().WithArguments(
                _horizontalWelcome,
                _verticalWelcome);
        }
    }
}
