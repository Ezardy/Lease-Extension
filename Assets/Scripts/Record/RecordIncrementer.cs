using Aniki.Character;
using Aniki.UI;
using MessagePipe;
using R3;
using System;
using LeaseExtension;
using LeaseExtension.Gameplay.Contract.Message;

namespace Aniki.Record {
	internal class RecordIncrementer : IDisposable {
		private readonly IDisposable		disposable;

		public RecordIncrementer(IRecordModel model,
			ICharacterModel characterModel,
			ISubscriber<RestartRequested> resetSubscriber,
			ISubscriber<BarPassed> barPassedSubscriber) {
			IDisposable	d2 = resetSubscriber.Subscribe(_ => model.SetRecord());
			IDisposable	d1 = barPassedSubscriber.Subscribe(_ => {
				if (characterModel.State != CharacterState.FALL)
					model.Increment();
			});
			disposable = Disposable.Combine(d1, d2);
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
