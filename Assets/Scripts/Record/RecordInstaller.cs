using UnityEngine;
using Zenject;

namespace Aniki.Record {
	[CreateAssetMenu(fileName = "RecordInstaller", menuName = "Installers/Record Installer")]
	public class RecordInstaller : ScriptableObjectInstaller<RecordInstaller> {
		[SerializeField] private RecordSave	recordSave;

		public override void	InstallBindings() {
			recordSave.Load();

			Container.BindInterfacesTo<RecordModel>().AsSingle().WithArguments(recordSave.Data.record);
			Container.QueueForInject(recordSave);
			Container.BindInterfacesTo<RecordSave>().FromInstance(recordSave);
			Container.BindInterfacesTo<RecordViewModel>().AsSingle();
		}
	}
}