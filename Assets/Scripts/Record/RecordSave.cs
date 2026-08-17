using Aniki.Save;
using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.Record {
	[CreateAssetMenu(fileName = "RecordSave", menuName = "Scriptable Objects/Saves/Record")]
	internal class RecordSave : ASave<RecordSave.Record, IRecordModel>, IInitializable, IDisposable {
		[Serializable]
		public struct Record {
			public uint	record;
		}

		private IRecordModel	record;
		private IDisposable		disposable;

		[Inject]
		public void	Init(IRecordModel record) {
			this.record = record;
		}

		public void	Initialize() {
			Init();
			disposable = record.RecordChanged.Subscribe(r => {
				data.record = r;
				Save();
			});
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
