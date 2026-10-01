using UnityEngine;
using Zenject;

namespace LeaseExtension.Input
{
    [CreateAssetMenu(fileName = "InputInstaller", menuName = "Installers/Input Installer")]
    internal class InputInstaller : ScriptableObjectInstaller<InputInstaller>
    {
        [SerializeField] private InputSettings _inputSettings;

        public override void InstallBindings()
        {
            Container.QueueForInject(_inputSettings);
        }
    }
}
