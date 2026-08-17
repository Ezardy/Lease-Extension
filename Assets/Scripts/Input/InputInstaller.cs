using UnityEngine;
using Zenject;

namespace Aniki.Input {
	[CreateAssetMenu(fileName = "InputInstaller", menuName = "Installers/Input Installer")]
	internal class InputInstaller : ScriptableObjectInstaller<InputInstaller> {
		[SerializeField] private InputSettings	inputSettings;

		public override void	InstallBindings() {
			Container.QueueForInject(inputSettings);
		}
	}
}