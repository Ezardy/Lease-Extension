using Aniki.Character;
using Aniki.Record;
using Aniki.Wallet;
using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Aniki.UI {
	internal class OverViewModel : IOverViewModel, IDisposable {
		private readonly IOverView					view;
		private readonly ICharacterModel			characterModel;
		private readonly IRecordModel				recordModel;
		private readonly IWalletModel				walletModel;
		private readonly IPublisher<AcceptSentenceMessage>	publisher;

		public OverViewModel(IOverView view, ICharacterModel characterModel,
			IRecordModel recordModel, IWalletModel walletModel,
			IPublisher<AcceptSentenceMessage> publisher) {
			this.view = view;
			this.characterModel = characterModel;
			this.recordModel = recordModel;
			this.walletModel = walletModel;
			this.publisher = publisher;

			UniTask.WaitWhile(() => view.Root == null).ContinueWith(Initialize).Forget();
		}

		[CreateProperty]
		public StyleEnum<DisplayStyle>	OverDisplayStyle =>
			characterModel.State == CharacterState.OVER
			? DisplayStyle.Flex
			: DisplayStyle.None;

		[CreateProperty]
		public StyleEnum<DisplayStyle>	NewRecordDisplayStyle =>
			recordModel.BarsPassed > recordModel.Record
			? DisplayStyle.Flex
			: DisplayStyle.None;

		[CreateProperty]
		public uint	BarsPassed => recordModel.BarsPassed;

		[CreateProperty]
		public uint	Earned => walletModel.Earned;

		private void	Initialize() {
			view.Root.dataSource = this;
			view.AcceptButton.clicked += OnClick;
		}

		public void	Dispose() {
			if (view.Root != null)
				view.AcceptButton.clicked -= OnClick;
		}

		private void	OnClick() {
			publisher.Publish(new());
		}
	}
}
