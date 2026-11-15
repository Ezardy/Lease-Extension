using Cysharp.Threading.Tasks;

namespace LeaseExtension.Common.Contract
{
    public interface IAsyncLoadable
    {
        public UniTask<bool> LoadAsync();
    }
}