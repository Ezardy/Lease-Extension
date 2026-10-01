using System.Collections.Generic;
using LeaseExtension.Input.Contract;
using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace LeaseExtension.Input
{
    [CreateAssetMenu(fileName = "InputSettings", menuName = "Scriptable Objects/Input Settings")]
    public class InputSettings : ScriptableObject
    {
        [SerializeField]
        private InputActionReference _punchActionReference;

        [SerializeField]
        private InputActionReference _tapActionReference;
        private IPublisher<PunchRequested> _punchPublisher;
        private IPublisher<Tapped> _tapPublisher;
        private InputAction _punchAction;
        private InputAction _tapAction;

        private void OnEnable()
        {
            if (_punchActionReference != null)
            {
                _punchAction = _punchActionReference.action;
                _punchAction.Enable();
                _punchAction.performed += PunchPerformed;
            }

            if (_tapActionReference != null)
            {
                _tapAction = _tapActionReference.action;
                _tapAction.Enable();
                _tapAction.performed += TapPerformed;
            }
        }

        private void OnDisable()
        {
            if (_punchAction != null)
            {
                _punchAction.performed -= PunchPerformed;
                _punchAction.Disable();
            }

            if (_tapAction != null)
            {
                _tapAction.performed -= TapPerformed;
                _tapAction.Disable();
            }
        }

        [Inject]
        public void Init(IPublisher<PunchRequested> punchPublisher, IPublisher<Tapped> tapPublisher)
        {
            _punchPublisher = punchPublisher;
            _tapPublisher = tapPublisher;
        }

        private void PunchPerformed(InputAction.CallbackContext context)
        {
            if (context.control.device is Pointer pointer)
            {
                List<RaycastResult> hits = new();
                EventSystem.current.RaycastAll(
                    new(EventSystem.current) { position = pointer.position.ReadValue() },
                    hits);
                if (hits.Count == 0)
                    _punchPublisher.Publish(new());
            }
            else
            {
                _punchPublisher.Publish(new());
            }
        }

        private void TapPerformed(InputAction.CallbackContext context)
        {
            _tapPublisher.Publish(new());
        }
    }
}
