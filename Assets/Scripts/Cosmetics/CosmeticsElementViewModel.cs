using Aniki.Common;
using Cysharp.Threading.Tasks;
using R3;
using System;
using Zenject;

namespace Aniki.Cosmetics {
	internal class CosmeticsElementViewModel<T> : ICosmeticsElementViewModel, ITyped<T>, IInitializable, IDisposable where T : class {
		private readonly ICosmeticsItemModelDatabase	hairstyleDatabase;
		private readonly ICosmeticsElementModel		cosmeticsModel;
		private readonly ICosmeticsElementView		cosmeticsView;

		private ICosmeticsSpriteHandles	handles;
		private IDisposable				disposable;

		public	CosmeticsElementViewModel(ICosmeticsItemModelDatabase database, ICosmeticsElementModel cosmeticsModel, ICosmeticsElementView cosmeticsView) {
			hairstyleDatabase = database;
			this.cosmeticsModel = cosmeticsModel;
			this.cosmeticsView = cosmeticsView;
		}
		
		private async UniTaskVoid	Set(string id) {
			handles?.Release();
			handles = hairstyleDatabase.GetCosmeticsItemModel(id).GetSpriteHandles();
			await handles.LoadTask();
			cosmeticsView.Set(handles.Back, handles.Front);
		}

		public void	Initialize() {
			disposable = cosmeticsModel.IdChanged.Subscribe(h => Set(h).Forget());
		}

		public void	Dispose() {
			cosmeticsView.Set(null, null);
			handles.Release();
			disposable?.Dispose();
		}
	}
}
