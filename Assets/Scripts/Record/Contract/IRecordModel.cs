using R3;

namespace LeaseExtension.Record.Contract
{
    public interface IRecordModel
    {
        public uint Record { get; }
        public uint BarsPassed { get; }

        public Observable<uint> RecordChanged { get; }
        public Observable<uint> BarsPassedChanged { get; }

        public void Increment();

        public void SetRecord();
    }
}
