using R3;

namespace Aniki.Record {
	public interface IRecordModel {
		public uint				Record { get; }
		public uint				BarsPassed { get; }

		public void	Increment();
		public void	SetRecord();

		public Observable<uint>	RecordChanged { get; }
		public Observable<uint>	BarsPassedChanged { get; }
	}
}
