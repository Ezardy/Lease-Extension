using System;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.World.Contract;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Floor
{
    internal class FloorViewModel : MonoBehaviour
    {
        private static readonly int _runHash = Animator.StringToHash("Run");
        private static readonly int _wallHash = Animator.StringToHash("Wall");
        private static readonly int _floorHash = Animator.StringToHash("Floor");
        [SerializeField] private float _speed = 1;
        private Observable<GameplayState> _subscriber;
        private bool _over;
        private IDisposable _runSubscription;
        private Animator _animator;

        [Inject]
        public void Init(
            IWorldModel worldModel,
            IFloorView floorView,
            Observable<GameplayState> subscriber,
            Animator animator)
        {
            worldModel.SpeedChanged.Subscribe(s => floorView.Speed = s / (1 + worldModel.Perspective) * 2).AddTo(this);
            worldModel.PerspectiveChanged.Subscribe(s => floorView.Perspective = s).AddTo(this);
            Observable.EveryValueChanged(this, x => x._speed).Subscribe(s => worldModel.SpeedAmplifier = s).AddTo(this);
            _subscriber = subscriber;
            _animator = animator;
            subscriber.Subscribe(s =>
            {
                switch (s)
                {
                    case GameplayState.Idle:
                        Idle();
                        break;
                    case GameplayState.Fall:
                        Wall();
                        break;
                    case GameplayState.Over:
                        Floor();
                        break;
                }
            }).AddTo(this);
        }

        private void Idle()
        {
            _over = false;
            _runSubscription = _subscriber.Subscribe(s =>
            {
                if (s == GameplayState.Punch)
                {
                    Run();
                }
            });
        }

        private void Run()
        {
            _runSubscription.Dispose();
            _animator.SetTrigger(_runHash);
        }

        private void Wall()
        {
            _over = true;
            _animator.SetTrigger(_wallHash);
        }

        private void Floor()
        {
            if (!_over)
            {
                _animator.SetTrigger(_floorHash);
            }
        }

        private void OnDestroy()
        {
            _runSubscription?.Dispose();
        }
    }
}