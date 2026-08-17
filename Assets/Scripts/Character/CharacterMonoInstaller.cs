using Aniki.Character;
using Aniki.State;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

internal class CharacterMonoInstaller : MonoInstaller {
	[SerializeField] private List<MonoBehaviour>	forInject;
	[SerializeField] private Rigidbody2D			characterRigidbody;

	public override void	InstallBindings() {
		InstallStates();

		Container.BindInterfacesTo<CharacterStateMachine>().AsSingle();
		Container.BindInstance(characterRigidbody);

		QueueForInject();
	}

	private void	QueueForInject() {
		foreach (MonoBehaviour c in forInject)
			Container.QueueForInject(c);
	}

	private void	InstallStates() {
		Container.BindFactory<CharacterState, StatePublisher<CharacterState>, StatePublisher<CharacterState>.Factory>();
		Container.Bind<CollisionCheckStateBase>().AsTransient();

		Container.BindFactory<IdleState, IdleState.Factory>().FromPoolableMemoryPool();
		Container.BindFactory<float, PunchState, PunchState.Factory>().FromPoolableMemoryPool();
		Container.BindFactory<WaitState, WaitState.Factory>().FromPoolableMemoryPool();
		Container.BindFactory<FallState, FallState.Factory>().FromPoolableMemoryPool();
		Container.BindFactory<OverState, OverState.Factory>().FromPoolableMemoryPool();
	}
}