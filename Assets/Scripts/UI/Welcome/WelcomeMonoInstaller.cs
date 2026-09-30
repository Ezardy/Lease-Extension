using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;

namespace LeaseExtension.UI.Welcome
{
    internal class WelcomeMonoInstaller : MonoInstaller
    {
        [SerializeField]
        [FormerlySerializedAs("panelRenderer")]
        private PanelRenderer _panelRenderer;

        [SerializeField]
        [FormerlySerializedAs("horizontalWelcome")]
        private AssetReferenceSprite _horizontalWelcome;

        [SerializeField]
        [FormerlySerializedAs("verticalWelcome")]
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
