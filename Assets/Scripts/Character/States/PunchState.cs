using Aniki.State;
using Aniki.World;
using UnityEngine;
using Zenject;

namespace Aniki.Character {
	internal class PunchState : APoolablePublishingState<float, PunchState, ICharacterContext, CharacterState> {
		private readonly CollisionCheckStateBase	stateBase;
		private readonly Rigidbody2D				rigidBody;
		private readonly WaitState.Factory			waitFactory;
		private readonly ICharacterModel			characterModel;
		private readonly IWorldModel				worldModel;

		private float	height;
		private float	timestamp;

		public PunchState(ICharacterContext context,
			CollisionCheckStateBase stateBase,
			StatePublisher<CharacterState>.Factory publisherFactory,
			WaitState.Factory waitFactory,
			Rigidbody2D rigidBody, ICharacterModel characterModel,
			IWorldModel worldModel)
			: base(context, publisherFactory.Create(CharacterState.PUNCH)) {
			this.stateBase = stateBase;
			this.rigidBody = rigidBody;
			this.waitFactory = waitFactory;
			this.characterModel = characterModel;
			this.worldModel = worldModel;
		}

		public override void	Start() {
			stateBase.Start();
			base.Start();
			rigidBody.AddForce((Mathf.Sqrt(-2 * worldModel.Gravity * height) - rigidBody.linearVelocityY) * rigidBody.mass * Vector2.up, ForceMode2D.Impulse);
			timestamp = Time.fixedTime + characterModel.PunchPeriod;
		}

		public override void	Dispose() {
			base.Dispose();
			stateBase.Dispose();
		}

		public override void Update() {
			if (timestamp < Time.fixedTime)
				context.State = waitFactory.Create();
		}

		public override void	OnSpawned(float param, IMemoryPool pool) {
			base.OnSpawned(param, pool);
			height = param;
		}
	}
}
