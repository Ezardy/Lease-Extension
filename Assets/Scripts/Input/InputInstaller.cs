using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Input
{
    [CreateAssetMenu(fileName = "InputInstaller", menuName = "Installers/Input Installer")]
    [MovedFrom("Aniki.Input")]
    internal class InputInstaller : ScriptableObjectInstaller<InputInstaller>
    {
        [SerializeField]
        [FormerlySerializedAs("inputSettings")]
        private InputSettings _inputSettings;

        public override void InstallBindings()
        {
            Container.QueueForInject(_inputSettings);
        }
    }
}
