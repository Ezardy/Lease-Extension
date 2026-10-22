using Cysharp.Threading.Tasks;
using LeaseExtension.Common.Contract;
using Zenject;

namespace LeaseExtension.Common.Utilities
{
    public class AsyncPlaceholderFactory<T> : PlaceholderFactory<T> where T : IAsyncLoadable
    {
        public override async UniTask<T> CreateAsync()
        {
            T instance = Create();
            await instance.LoadAsync();
            return instance;
        }
    }
}