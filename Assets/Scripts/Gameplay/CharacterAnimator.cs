using MessagePipe;
using R3;
using UnityEngine;
using Zenject;

namespace Aniki.Character {
	public class CharacterAnimator : MonoBehaviour {
		[SerializeField] private Animator	babAnimator;
		[SerializeField] private Animator	pupAnimator;

		private static readonly int	overHash = Animator.StringToHash("Over");
		private static readonly int	punchHash = Animator.StringToHash("Punch");

		[Inject]
		public void	Init(ISubscriber<CharacterState> subscriber) {
			subscriber.AsObservable(CharacterStateFilter.Idle).ToObservable().Skip(1).Subscribe((_) => Idle()).AddTo(this);
			subscriber.Subscribe((_) => Punch(), CharacterStateFilter.Punch).AddTo(this);
			subscriber.Subscribe((_) => Wait(), CharacterStateFilter.Wait).AddTo(this);
			subscriber.Subscribe((_) => Over(), CharacterStateFilter.Over).AddTo(this);
			subscriber.Subscribe((_) => Fall(), CharacterStateFilter.Fall).AddTo(this);
		}

		private void	Idle() {
			babAnimator.SetBool(overHash, false);
			babAnimator.SetBool(punchHash, false);
			pupAnimator.SetBool(overHash, false);
			pupAnimator.SetBool(punchHash, false);
		}

		private void	Punch() {
			babAnimator.SetBool(punchHash, true);
			pupAnimator.SetBool(punchHash, true);
		}

		private void	Wait() {
			babAnimator.SetBool(punchHash, false);
			pupAnimator.SetBool(punchHash, false);
		}

		private void	Over() {
			pupAnimator.SetBool(overHash, true);
			babAnimator.SetBool(punchHash, false);
			babAnimator.SetBool(overHash, true);
		}

		private void	Fall() {
			pupAnimator.SetBool(punchHash, false);
			babAnimator.SetBool(punchHash, false);
			babAnimator.SetBool(overHash, true);
		}
	}
}
