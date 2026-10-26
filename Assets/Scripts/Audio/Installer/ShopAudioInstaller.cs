using LeaseExtension.Audio.Background;
using LeaseExtension.Audio.Background.States;
using LeaseExtension.SceneManagement.Contract.Message;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Audio.Installer
{
    [CreateAssetMenu(fileName = "ShopAudioInstaller", menuName = "Installers/Audio/Shop")]
    internal class
        ShopAudioInstaller : ScriptableObjectInstaller<ShopAudioInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<ShopAudioState, ShopAudioState.Factory>().FromPoolableMemoryPool();
            Container.BindInterfacesTo<CrossSceneAudioStateTransition<ShopAudioState, ShopAudioState.Factory>>()
                .AsSingle().WithArguments(FocusedScene.Shop);
        }
    }
}