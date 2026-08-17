using Aniki.Character;
using Aniki.Record;
using Aniki.Wallet;
using Cysharp.Threading.Tasks;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Aniki.UI {
	internal class GameViewModel : IGameViewModel {
		private readonly ICharacterModel	characterModel;
		private readonly IRecordModel		recordModel;
		private readonly IWalletModel		walletModel;

		[CreateProperty] public StyleEnum<DisplayStyle>	GameDisplayStyle =>
			characterModel.State != CharacterState.IDLE
			&& characterModel.State != CharacterState.OVER
			? DisplayStyle.Flex : DisplayStyle.None;

		[CreateProperty] public uint BarsPassed => recordModel.BarsPassed;

		[CreateProperty] public uint Earned => walletModel.Earned;

		public GameViewModel(IGameView view, ICharacterModel characterModel,
			IRecordModel recordModel, IWalletModel walletModel) {
			this.characterModel = characterModel;
			this.recordModel = recordModel;
			this.walletModel = walletModel;

			UniTask.WaitWhile(() => view.Root == null).ContinueWith(() => view.Root.dataSource = this).Forget();
		}
	}
}
