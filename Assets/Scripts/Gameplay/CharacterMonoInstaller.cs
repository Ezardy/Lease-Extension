using System.Collections.Generic;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Gameplay.States;
using LeaseExtension.State;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Gameplay
{
    internal class CharacterMonoInstaller : MonoInstaller
    {
        [SerializeField]
        [FormerlySerializedAs("forInject")]
        private List<MonoBehaviour> _forInject;

        [SerializeField]
        [FormerlySerializedAs("characterRigidbody")]
        private Rigidbody2D _characterRigidbody;

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
            Container.BindFactory<CharacterState, StatePublisher<CharacterState>, StatePublisher<CharacterState>.Factory>();
            Container.Bind<CollisionCheckStateBase>().AsTransient();
            Container.BindFactory<IdleState, IdleState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<float, PunchState, PunchState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<WaitState, WaitState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<FallState, FallState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<OverState, OverState.Factory>().FromPoolableMemoryPool();
        }
    }
}
