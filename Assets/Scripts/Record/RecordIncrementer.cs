using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Record.Contract;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using R3;

namespace LeaseExtension.Record
{
    [UsedImplicitly]
    internal class RecordIncrementer : IDisposable
    {
        private readonly IDisposable _disposable;

        public RecordIncrementer(
            IRecordModel model,
            ReadOnlyReactiveProperty<CharacterState> characterState,
            ISubscriber<RestartRequested> resetSubscriber,
            ISubscriber<BarPassed> barPassedSubscriber)
        {
            IDisposable d2 = resetSubscriber.Subscribe(_ => model.SetRecord());
            IDisposable d1 = barPassedSubscriber.Subscribe(_ =>
            {
                if (characterState.CurrentValue != CharacterState.Fall)
                    model.Increment();
            });
            _disposable = Disposable.Combine(d1, d2);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
