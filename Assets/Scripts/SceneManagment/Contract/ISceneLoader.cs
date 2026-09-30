using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace LeaseExtension.SceneManagment.Contract
{
    public interface ISceneLoader
    {
        public UniTask LoadAsync(AssetReference scene);

        public void Activate();
    }
}
