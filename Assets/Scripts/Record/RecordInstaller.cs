using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Record
{
    [CreateAssetMenu(fileName = "RecordInstaller", menuName = "Installers/Record Installer")]
    [MovedFrom("Aniki.Record")]
    public class RecordInstaller : ScriptableObjectInstaller<RecordInstaller>
    {
        [SerializeField]
        [FormerlySerializedAs("recordSave")]
        private RecordSave _recordSave;

        public override void InstallBindings()
        {
            _recordSave.Load();
            Container.BindInterfacesTo<RecordModel>().AsSingle().WithArguments(_recordSave.Data.Value);
            Container.QueueForInject(_recordSave);
            Container.BindInterfacesTo<RecordSave>().FromInstance(_recordSave);
            Container.BindInterfacesTo<RecordIncrementer>().AsSingle();
        }
    }
}
