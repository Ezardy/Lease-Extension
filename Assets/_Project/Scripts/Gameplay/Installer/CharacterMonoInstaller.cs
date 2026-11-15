using LeaseExtension.Common.Utilities;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.States;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Gameplay.Installer
{
    internal class CharacterMonoInstaller : MonoInstaller
    {
        [SerializeField] private Ref<ICharacterModel> _characterModel;
        [SerializeField] private Rigidbody2D _characterRigidbody;

        public override void InstallBindings()
        {
            InstallStates();
            Container.BindInterfacesTo<CharacterStateMachine>().AsSingle();
            Container.BindInstance(_characterRigidbody);
        }

        private void InstallStates()
        {
            Container.BindInstance(_characterModel.I);
            Container.Bind<CollisionCheckStateBase>().AsTransient();
            Container.BindFactory<IdleState, IdleState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<float, PunchState, PunchState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<WaitState, WaitState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<FallState, FallState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<OverState, OverState.Factory>().FromPoolableMemoryPool();
        }
    }
}