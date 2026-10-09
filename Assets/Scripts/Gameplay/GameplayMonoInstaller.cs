using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Gameplay.States;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Gameplay
{
    internal class GameplayMonoInstaller : MonoInstaller
    {
        [SerializeField] private Ref<ICharacterModel> _characterModel;
        [SerializeField] private List<MonoBehaviour> _forInject;
        [SerializeField] private Rigidbody2D _characterRigidbody;

        public override void InstallBindings()
        {
            InstallStates();
            Container.BindInterfacesTo<CharacterStateMachine>().AsSingle();
            Container.BindInstance(_characterRigidbody);
            QueueForInject();
        }

        private void QueueForInject()
        {
            foreach (MonoBehaviour c in _forInject)
                Container.QueueForInject(c);
        }

        private void InstallStates()
        {
            Container.BindInstance(_characterModel.I);
            Container.Bind<ReactiveProperty<CharacterState>>().AsSingle();
            Container.Bind<ReadOnlyReactiveProperty<CharacterState>>().To<ReactiveProperty<CharacterState>>()
                .FromResolve();
            Container.Bind<Observable<CharacterState>>().To<ReactiveProperty<CharacterState>>()
                .FromResolve();
            Container.Bind<CollisionCheckStateBase>().AsTransient();
            Container.BindFactory<IdleState, IdleState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<float, PunchState, PunchState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<WaitState, WaitState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<FallState, FallState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<OverState, OverState.Factory>().FromPoolableMemoryPool();
        }
    }
}
