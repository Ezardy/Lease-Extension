using LeaseExtension.Common.Utilities;
using LeaseExtension.Gameplay.Contract;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Gameplay
{
    [CreateAssetMenu(fileName = "CharacterInstaller", menuName = "Installers/Character Installer")]
    [MovedFrom("Aniki.Character")]
    public class CharacterInstaller : ScriptableObjectInstaller<CharacterInstaller>
    {
        [SerializeField]
        [FormerlySerializedAs("characterModel")]
        private IRef<ICharacterModel> _characterModel;

        public override void InstallBindings()
        {
            Container.BindInstance(_characterModel.I);
        }
    }
}
