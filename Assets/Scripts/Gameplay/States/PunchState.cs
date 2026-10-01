using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.State;
using LeaseExtension.World.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Gameplay.States
{
    [UsedImplicitly]
    internal class PunchState : APoolablePublishingState<float, PunchState, ICharacterContext, CharacterState>
    {
        private readonly CollisionCheckStateBase _stateBase;
        private readonly Rigidbody2D _rigidBody;
        private readonly WaitState.Factory _waitFactory;
        private readonly ICharacterModel _characterModel;
        private readonly IWorldModel _worldModel;
        private float _height;
        private float _timestamp;

        public PunchState(
            ICharacterContext context,
            CollisionCheckStateBase stateBase,
            StatePublisher<CharacterState>.Factory publisherFactory,
            WaitState.Factory waitFactory,
            Rigidbody2D rigidBody,
            ICharacterModel characterModel,
            IWorldModel worldModel) : base(
            context,
            publisherFactory.Create(CharacterState.Punch))
        {
            _stateBase = stateBase;
            _rigidBody = rigidBody;
            _waitFactory = waitFactory;
            _characterModel = characterModel;
            _worldModel = worldModel;
        }

        public override void Start()
        {
            _stateBase.Start();
            base.Start();
            _rigidBody.AddForce((Mathf.Sqrt(
-2 * _worldModel.Gravity * _height) - _rigidBody.linearVelocityY) * _rigidBody.mass * Vector2.up, ForceMode2D.Impulse);
            _timestamp = Time.fixedTime + _characterModel.PunchPeriod;
        }

        public override void Update()
        {
            if (_timestamp < Time.fixedTime)
                Context.State = _waitFactory.Create();
        }

        public override void Dispose()
        {
            base.Dispose();
            _stateBase.Dispose();
        }

        public override void OnSpawned(float param, IMemoryPool pool)
        {
            base.OnSpawned(param, pool);
            _height = param;
        }
    }
}
