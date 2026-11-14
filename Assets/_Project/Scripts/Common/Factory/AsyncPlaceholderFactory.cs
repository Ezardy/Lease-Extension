using Cysharp.Threading.Tasks;
using LeaseExtension.Common.Contract;
using Zenject;

namespace LeaseExtension.Common.Factory
{
    public class AsyncPlaceholderFactory<T> : PlaceholderFactory<T> where T : IAsyncLoadable
    {
        public override async UniTask<T> CreateAsync()
        {
            T instance = Create();
            bool succeed = await instance.LoadAsync();
            return succeed ? instance : default;
        }
    }
    
    public class AsyncPlaceholderFactory<TP, T> : PlaceholderFactory<TP, T> where T : IAsyncLoadable
    {
        public override async UniTask<T> CreateAsync(TP p)
        {
            T instance = Create(p);
            bool succeed = await instance.LoadAsync();
            return succeed ? instance : default;
        }
    }
}