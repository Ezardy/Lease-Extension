using Aniki.Character;
using Aniki.Record;
using Aniki.Wallet;
using Cysharp.Threading.Tasks;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Aniki.UI {
	internal class MainViewModel : IMainViewModel {
		private readonly ICharacterModel	characterModel;
		private readonly IRecordModel		recordModel;

		[CreateProperty] public StyleEnum<DisplayStyle>	MainDisplayStyle =>
			characterModel.State == CharacterState.IDLE
			? DisplayStyle.Flex
			: DisplayStyle.None;

		[CreateProperty] public uint	Record => recordModel.Record;

		public MainViewModel(IMainView view, IWalletModel walletModel,
			ICharacterModel characterModel, IRecordModel recordModel) {
			UniTask.WaitWhile(() => view.Root == null).ContinueWith(() => {
				view.Root.dataSource = this;
				view.EarnedGroup.dataSource = walletModel;
			}).Forget();
			this.characterModel = characterModel;
			this.recordModel = recordModel;
		}
	}
}
