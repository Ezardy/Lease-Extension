using System;
using LeaseExtension.Input.Relay;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Input.Installer
{
    [CreateAssetMenu(fileName = "InputInstaller", menuName = "Installers/Input/Input")]
    internal class InputInstaller : ScriptableObjectInstaller<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(LeaseExtensionInput), typeof(IDisposable)).To<LeaseExtensionInput>().AsSingle();
        }
    }
}