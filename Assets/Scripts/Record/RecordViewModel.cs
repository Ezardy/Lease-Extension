using Aniki.Character;
using Aniki.UI;
using Aniki.World;
using MessagePipe;
using R3;
using System;

namespace Aniki.Record {
	internal class RecordViewModel : IDisposable {
		private readonly IDisposable		disposable;

		public RecordViewModel(IRecordModel model,
			ICharacterModel characterModel,
			ISubscriber<AcceptSentenceMessage> resetSubscriber,
			ISubscriber<BarPassedMessage> barPassedSubscriber) {
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
