using LeaseExtension.UI.Game.Main;
using LeaseExtension.UI.Game.Over;
using LeaseExtension.UI.Game.Race;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;

namespace LeaseExtension.UI.Game
{
    internal class UIMonoInstaller : MonoInstaller
    {
        [SerializeField]
        [FormerlySerializedAs("panelRenderer")]
        private PanelRenderer _panelRenderer;

        public override void InstallBindings()
        {
            Container.BindInstance(_panelRenderer);
            Container.BindInterfacesTo<MainView>().AsSingle();
            Container.BindInterfacesTo<MainViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesTo<OverView>().AsSingle();
            Container.BindInterfacesTo<OverViewModel>().AsSingle();
            Container.BindInterfacesTo<RaceView>().AsSingle();
            Container.BindInterfacesTo<RaceViewModel>().AsSingle().NonLazy();
        }
    }
}
