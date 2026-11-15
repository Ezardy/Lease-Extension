using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace LeaseExtension.UI.Shop
{
    internal class ShopUIMonoInstaller : MonoInstaller
    {
        [SerializeField] private PanelRenderer _panelRenderer;

        public override void InstallBindings()
        {
            Container.BindInstance(_panelRenderer);
            Container.BindInterfacesTo<ShopView>().AsSingle();
            Container.BindInterfacesTo<ShopViewModel>().AsSingle();
        }
    }
}