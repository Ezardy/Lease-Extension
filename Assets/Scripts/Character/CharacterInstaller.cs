using Aniki.Common;
using UnityEngine;
using Zenject;

namespace Aniki.Character {
	[CreateAssetMenu(fileName = "CharacterInstaller", menuName = "Installers/Character Installer")]
	public class CharacterInstaller : ScriptableObjectInstaller<CharacterInstaller> {
		[SerializeField] private IRef<ICharacterModel>	characterModel;

		public override void	InstallBindings() {
			Container.BindInstance(characterModel.I);
		}
	}
}