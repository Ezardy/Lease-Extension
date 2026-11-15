using LeaseExtension.Gameplay.Contract.Message;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Gameplay
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _granAnimator;
        [SerializeField] private Animator _grandsonAnimator;
        private static readonly int _overHash = Animator.StringToHash("Over");
        private static readonly int _punchHash = Animator.StringToHash("Punch");
        private Observable<GameplayState> _subscriber;

        [Inject]
        public void Init(Observable<GameplayState> subscriber)
        {
            _subscriber = subscriber;
        }

        private void Idle()
        {
            _granAnimator.SetBool(_overHash, false);
            _granAnimator.SetBool(_punchHash, false);
            _grandsonAnimator.SetBool(_overHash, false);
            _grandsonAnimator.SetBool(_punchHash, false);
        }

        private void Punch()
        {
            _granAnimator.SetBool(_punchHash, true);
            _grandsonAnimator.SetBool(_punchHash, true);
        }

        private void Wait()
        {
            _granAnimator.SetBool(_punchHash, false);
            _grandsonAnimator.SetBool(_punchHash, false);
        }

        private void Over()
        {
            _grandsonAnimator.SetBool(_overHash, true);
            _granAnimator.SetBool(_punchHash, false);
            _granAnimator.SetBool(_overHash, true);
        }

        private void Fall()
        {
            _grandsonAnimator.SetBool(_punchHash, false);
            _granAnimator.SetBool(_punchHash, false);
            _granAnimator.SetBool(_overHash, true);
        }

        private void Start()
        {
            _subscriber.Subscribe(s =>
            {
                switch (s)
                {
                    case GameplayState.Idle:
                        Idle();
                        break;
                    case GameplayState.Wait:
                        Wait();
                        break;
                    case GameplayState.Punch:
                        Punch();
                        break;
                    case GameplayState.Fall:
                        Fall();
                        break;
                    default:
                        Over();
                        break;
                }
            }).AddTo(this);
        }
    }
}