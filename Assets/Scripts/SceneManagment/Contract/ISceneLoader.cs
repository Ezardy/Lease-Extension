using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Aniki.SceneManagment {
	public interface ISceneLoader {
		public UniTask	LoadAsync(AssetReference scene);
		public void		Activate();
	}
}
