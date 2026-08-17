using MessagePipe;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Aniki.Input {
	[CreateAssetMenu(fileName = "InputSettings", menuName = "Scriptable Objects/Input Settings")]
	public class InputSettings : ScriptableObject {
		[SerializeField] private InputActionReference	punchActionReference;
		[SerializeField] private InputActionReference	tapActionReference;

		private IPublisher<PunchInputMessage>	punchPublisher;
		private IPublisher<TapInputMessage>		tapPublisher;

		private InputAction	punchAction;
		private InputAction	tapAction;

		[Inject]
		public void	Init(IPublisher<PunchInputMessage> punchPublisher,
			IPublisher<TapInputMessage> tapPublisher) {
			this.punchPublisher = punchPublisher;
			this.tapPublisher = tapPublisher;
		}

		private void	OnEnable() {
			if (punchActionReference != null) {
				punchAction = punchActionReference.action;
				punchAction.Enable();
				punchAction.performed += PunchPerformed;
			}
			if (tapActionReference != null) {
				tapAction = tapActionReference.action;
				tapAction.Enable();
				tapAction.performed += TapPerformed;
			}
		}

		private void	OnDisable() {
			if (punchAction != null) {
				punchAction.performed -= PunchPerformed;
				punchAction.Disable();
			}
			if(tapAction != null) {
				tapAction.performed -= TapPerformed;
				tapAction.Disable();
			}
		}

		private void	PunchPerformed(InputAction.CallbackContext context) {
			if (context.control.device is Pointer pointer) {
				List<RaycastResult>	hits = new();

				EventSystem.current.RaycastAll(
					new(EventSystem.current) { position = pointer.position.ReadValue() },
					hits
				);
				if (hits.Count == 0)
					punchPublisher.Publish(new());
			} else
				punchPublisher.Publish(new());
		}

		private void	TapPerformed(InputAction.CallbackContext context) {
			tapPublisher.Publish(new());
		}
	}
}
