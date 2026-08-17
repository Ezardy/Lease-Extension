using R3;

namespace Aniki.Record {
	public class RecordModel : IRecordModel {
		private readonly ReactiveProperty<uint>	record;
		private readonly ReactiveProperty<uint>	barsPassed;

		public Observable<uint>	RecordChanged => record;
		public Observable<uint>	BarsPassedChanged => barsPassed;

		public RecordModel(uint record) {
			this.record = new(record);
			barsPassed = new(0);
		}

		public uint	Record => record.CurrentValue;
		public uint	BarsPassed => barsPassed.CurrentValue;

		public void	Increment() {
			barsPassed.Value += 1;
		}

		public void	SetRecord() {
			if (Record < BarsPassed)
				record.Value = BarsPassed;
			barsPassed.Value = 0;
		}
	}
}
