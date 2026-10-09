using UnityEngine;
using Zenject;

namespace LeaseExtension.Record
{
    [CreateAssetMenu(fileName = "RecordInstaller", menuName = "Installers/Record Installer")]
    public class RecordInstaller : ScriptableObjectInstaller<RecordInstaller>
    {
        [SerializeField] private RecordSave _recordSave;

        public override void InstallBindings()
        {
            _recordSave.Load();
            Container.BindInterfacesTo<RecordModel>().AsSingle().WithArguments(_recordSave.Data.Value);
            Container.QueueForInject(_recordSave);
            Container.BindInterfacesTo<RecordSave>().FromInstance(_recordSave);
        }
    }
}
