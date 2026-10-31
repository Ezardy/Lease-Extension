using System;
using JetBrains.Annotations;
using LeaseExtension.Input.Contract;
using MessagePipe;
using UnityEngine.InputSystem;
using Zenject;

namespace LeaseExtension.Input.Relay
{
    [UsedImplicitly]
    internal class UIInputRelay : IInitializable, IDisposable
    {
        private readonly IPublisher<Submitted> _submitPublisher;
        private LeaseExtensionInput.UIActions _uiActions;

        public UIInputRelay(LeaseExtensionInput.UIActions actions, IPublisher<Submitted> submitPublisher)
        {
            _submitPublisher = submitPublisher;
            _uiActions = actions;
        }

        public void Initialize()
        {
            _uiActions.Enable();
            _uiActions.Submit.performed += SubmitPerformed;
        }

        public void Dispose()
        {
            _uiActions.Submit.performed -= SubmitPerformed;
            _uiActions.Disable();
        }

        private void SubmitPerformed(InputAction.CallbackContext context)
        {
            _submitPublisher.Publish(new());
        }
    }
}