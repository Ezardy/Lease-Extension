using System;
using LeaseExtension.Gameplay.Contract.Message;
using MessagePipe;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Gameplay
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _babAnimator;
        [SerializeField] private Animator _pupAnimator;
        private static readonly int _overHash = Animator.StringToHash("Over");
        private static readonly int _punchHash = Animator.StringToHash("Punch");

        [Inject]
        public void Init(ReadOnlyReactiveProperty<CharacterState> subscriber)
        {
            subscriber.Subscribe(s =>
            {
                switch (s)
                {
                    case CharacterState.Idle:
                        Idle();
                        break;
                    case CharacterState.Wait:
                        Wait();
                        break;
                    case CharacterState.Punch:
                        Punch();
                        break;
                    case CharacterState.Fall:
                        Fall();
                        break;
                    default:
                        Over();
                        break;
                }
            }).AddTo(this);
        }

        private void Idle()
        {
            _babAnimator.SetBool(_overHash, false);
            _babAnimator.SetBool(_punchHash, false);
            _pupAnimator.SetBool(_overHash, false);
            _pupAnimator.SetBool(_punchHash, false);
        }

        private void Punch()
        {
            _babAnimator.SetBool(_punchHash, true);
            _pupAnimator.SetBool(_punchHash, true);
        }

        private void Wait()
        {
            _babAnimator.SetBool(_punchHash, false);
            _pupAnimator.SetBool(_punchHash, false);
        }

        private void Over()
        {
            _pupAnimator.SetBool(_overHash, true);
            _babAnimator.SetBool(_punchHash, false);
            _babAnimator.SetBool(_overHash, true);
        }

        private void Fall()
        {
            _pupAnimator.SetBool(_punchHash, false);
            _babAnimator.SetBool(_punchHash, false);
            _babAnimator.SetBool(_overHash, true);
        }
    }
}
