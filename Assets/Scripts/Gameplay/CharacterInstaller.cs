using LeaseExtension.Common.Utilities;
using LeaseExtension.Gameplay.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Gameplay
{
    [CreateAssetMenu(fileName = "CharacterInstaller", menuName = "Installers/Character Installer")]
    public class CharacterInstaller : ScriptableObjectInstaller<CharacterInstaller>
    {
        [SerializeField] private Ref<ICharacterModel> _characterModel;

        public override void InstallBindings()
        {
            Container.BindInstance(_characterModel.I);
        }
    }
}
