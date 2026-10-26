using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.SceneManagement.Contract.Message;
using MessagePipe;

namespace LeaseExtension.UI.Shop
{
    [UsedImplicitly]
    internal class ShopViewModel : IShopViewModel, IDisposable
    {
        private readonly IShopView _view;
        private readonly IPublisher<FocusedScene> _sceneRequestPublisher;
        
        public ShopViewModel(IShopView view, IPublisher<FocusedScene> sceneRequestPublisher)
        {
            _view = view;
            _sceneRequestPublisher = sceneRequestPublisher;
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(() => view.Close.clicked += OnCloseClicked).Forget();
        }

        public void Dispose()
        {
            if (_view.Root != null)
            {
                _view.Close.clicked -= OnCloseClicked;
            }
        }

        private void OnCloseClicked()
        {
            _sceneRequestPublisher.Publish(FocusedScene.Game);
        }
    }
}