using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.Input.Contract;
using MessagePipe;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace LeaseExtension.Input.Relay
{
    [UsedImplicitly]
    internal class PlayerInputRelay : IInitializable, IDisposable
    {
        private readonly IPublisher<PunchRequested> _punchPublisher;
        private LeaseExtensionInput.PlayerActions _playerActions;

        public PlayerInputRelay(LeaseExtensionInput.PlayerActions actions, IPublisher<PunchRequested> punchPublisher)
        {
            _punchPublisher = punchPublisher;
            _playerActions = actions;
        }

        public void Initialize()
        {
            _playerActions.Enable();
            _playerActions.Punch.performed += PunchPerformed;
        }

        public void Dispose()
        {
            _playerActions.Punch.performed -= PunchPerformed;
            _playerActions.Disable();
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
                {
                    _punchPublisher.Publish(new());
                }
            }
            else
            {
                _punchPublisher.Publish(new());
            }
        }
    }
}