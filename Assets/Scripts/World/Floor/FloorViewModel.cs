using Aniki.Character;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class FloorViewModel : MonoBehaviour {

		private static readonly int	runHash = Animator.StringToHash("Run");
		private static readonly int	wallHash = Animator.StringToHash("Wall");
		private static readonly int	floorHash = Animator.StringToHash("Floor");

		[SerializeField] private float	speed = 1;

		private ISubscriber<CharacterState>	subscriber;
		private bool						over = false;
		private IDisposable					runSubscription;
		private Animator					animator;

		[Inject]
		public void	Init(IWorldModel worldModel, IFloorView floorView,
			ISubscriber<CharacterState> subscriber, Animator animator) {
			worldModel.SpeedChanged.Subscribe(s => floorView.Speed = s / (1 + worldModel.Perspective) * 2).AddTo(this);
			worldModel.PerspectiveChanged.Subscribe(s => floorView.Perspective = s).AddTo(this);
			Observable.EveryValueChanged(this, x => x.speed).Subscribe(s => worldModel.SpeedAmplifier = s).AddTo(this);

			this.subscriber = subscriber;
			this.animator = animator;
			subscriber.Subscribe(_ => Idle(), CharacterStateFilter.Idle).AddTo(this);
			subscriber.Subscribe(_ => Wall(), CharacterStateFilter.Fall).AddTo(this);
			subscriber.Subscribe(_ => Floor(), CharacterStateFilter.Over).AddTo(this);
		}

		private void	Idle() {
			over = false;
			runSubscription = subscriber.Subscribe(_ => Run(), CharacterStateFilter.Punch);
		}

		private void	Run() {
			runSubscription.Dispose();
			animator.SetTrigger(runHash);
		}

		private void	Wall() {
			over = true;
			animator.SetTrigger(wallHash);
		}

		private void	Floor() {
			if (!over)
				animator.SetTrigger(floorHash);
		}

		private void	OnDestroy() {
			runSubscription?.Dispose();
		}
	}
}
