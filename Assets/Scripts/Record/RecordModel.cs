using JetBrains.Annotations;
using LeaseExtension.Record.Contract;
using R3;

namespace LeaseExtension.Record
{
    [UsedImplicitly]
    public class RecordModel : IRecordModel
    {
        private readonly ReactiveProperty<uint> _record;
        private readonly ReactiveProperty<uint> _barsPassed;

        public Observable<uint> RecordChanged => _record;
        public Observable<uint> BarsPassedChanged => _barsPassed;
        public uint Record => _record.CurrentValue;
        public uint BarsPassed => _barsPassed.CurrentValue;

        public RecordModel(uint record)
        {
            _record = new(record);
            _barsPassed = new(0);
        }

        public void Increment()
        {
            _barsPassed.Value += 1;
        }

        public void SetRecord()
        {
            if (Record < BarsPassed)
                _record.Value = BarsPassed;
            _barsPassed.Value = 0;
        }
    }
}
